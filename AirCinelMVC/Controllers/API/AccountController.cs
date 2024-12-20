﻿using AirCinelMVC.Data;
using AirCinelMVC.Data.Dtos;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Helpers;
using AirCinelMVC.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AirCinelMVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConfiguration _configuration;

        public AccountController(
            ICountryRepository countryRepository,
            IUserRepository userRepository,
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IBlobHelper blobHelper,
            IConfiguration configuration
            )
        {
            _countryRepository = countryRepository;
            _userRepository = userRepository;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _blobHelper = blobHelper;
            _configuration = configuration;
        }

        [HttpPost("createtokenapi")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateTokenAPI([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var role = await _userHelper.GetRolesAsync(user);
                        var roleAsString = role.FirstOrDefault();

                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Role, roleAsString),
                            new Claim(ClaimTypes.Email, user.Email)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            Expiration = token.ValidTo,
                            UserId = user.Id,
                            UserName = user.UserName,
                        };

                        return this.Created(string.Empty, results);
                    }
                    else
                    {
                       return this.BadRequest("Invalid login.");
                    }
                }
            }

            return BadRequest();
        }


        [Authorize]
        [HttpGet("getuserimage")]
        public async Task<IActionResult> UserProfileImage()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var user = await _userRepository.GetUserByEmailAsync(userEmail);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userImagePath = user.ImageFullPath;

            return Ok(userImagePath);
        }

        [HttpPost("recoverpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> RecoverPasswordAPI([FromBody] RecoverPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new { Message = "User not found." });
            }

            var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

            var link = this.Url.Action(
                "ResetPassword",
                "Account",
                new { token = myToken }, protocol: HttpContext.Request.Scheme);

            Response response = _mailHelper.SendEmail(model.Email, "AirCinel - Password Reset",
                                                        $"<h1 style=\"color:#1E90FF;\">AirCinel Password Reset</h1>" +
                                                        $"<p>We received a request to reset your password. If you made this request, please click the link below to reset your password:</p>" +
                                                        $"<p><a href = \"{link}\" style=\"color:#FFA500; font-weight:bold;\">Reset Password</a></p>" +
                                                        $"<p>If you did not request a password reset, please ignore this email. Your account is still secure.</p>" +
                                                        $"<br>" +
                                                        $"<p>Best regards,</p>" +
                                                        $"<p>The AirCinel Team</p>" +
                                                        $"<p><small>This is an automated message. Please do not reply to this email.</small></p>");

            if (!response.IsSuccess)
            {
                return BadRequest(new { Message = "Error sending recovery email." });
            }

            return Ok(new { Message = "Password reset email sent successfully." });
        }

        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAPI([FromBody] ChangePasswordDto model)
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByEmailAsync(userEmail);

            if (user == null)
            {
                return Unauthorized(new { Message = "User not found." });
            }

            var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Error changing password. Make sure your password is strong enough." });
            }

            return Ok(new { Message = "Password changed successfully." });
        }

        //[HttpPost("register")]
        //[AllowAnonymous]
        //public async Task<IActionResult> RegisterAPI([FromBody] RegisterDto model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = await _userRepository.GetUserByEmailAsync(model.Username);
        //    if (user != null)
        //    {
        //        return BadRequest(new { Message = "The email is already registered." });
        //    }

        //    var city = await _countryRepository.GetCityAsync(model.CityId);
        //    if (city == null)
        //    {
        //        return BadRequest(new { Message = "Invalid city selected." });
        //    }

        //    user = new User
        //    {
        //        FirstName = model.FirstName,
        //        LastName = model.LastName,
        //        Email = model.Username,
        //        UserName = model.Username,
        //        Address = model.Address,
        //        PhoneNumber = model.PhoneNumber,
        //        CityId = model.CityId,
        //        ImageId = model.ImageId
        //    };

        //    if (model.ImageId != Guid.Empty)
        //    {
        //        var imageId = await _blobHelper.VerifyAndUploadImageAsync(model.ImageId, "users");
        //        user.ImageId = imageId; 
        //    }

        //    var result = await _userRepository.AddUserAsync(user, model.Password);
        //    if (result != IdentityResult.Success)
        //    {
        //        return BadRequest(new { Message = "The user couldn't be created.", Errors = result.Errors });
        //    }

        //    await _userHelper.AddUserToRoleAsync(user, "Customer");
        //    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

        //    string tokenLink = Url.Action("ConfirmEmail", "Account", new
        //    {
        //        userid = user.Id,
        //        token = myToken
        //    }, protocol: HttpContext.Request.Scheme);

        //    var response = _mailHelper.SendEmail(model.Username, "AirCinel - Confirm your Email",
        //        $"<h1 style=\"color:#1E90FF;\">Welcome to AirCinel!</h1>" +
        //        $"<p>Thank you for choosing AirCinel, your trusted airline for premium travel experiences.</p>" +
        //        $"<p>To complete your registration, please confirm your email address by clicking the link below:</p>" +
        //        $"<p><a href = \"{tokenLink}\" style=\"color:#FFA500; font-weight:bold;\">Confirm Email</a></p>" +
        //        $"<p>If you didn’t create this account, please disregard this email.</p><br>" +
        //        $"<p>Safe travels,</p>" +
        //        $"<p>The AirCinel Team</p>" +
        //        $"<p><small>This is an automated message. Please do not reply to this email.</small></p>"
        //    );

        //    if (!response.IsSuccess)
        //    {
        //        return StatusCode(500, "The registration email couldn't be sent.");
        //    }

        //    return Ok(new { Message = "User registered successfully. Please confirm your email." });
        //}

        [HttpPost("registerWithImage")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterWithImage([FromForm] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserByEmailAsync(model.Username);
            if (user != null)
            {
                return BadRequest(new { Message = "The email is already registered." });
            }

            var city = await _countryRepository.GetCityAsync(model.CityId);
            if (city == null)
            {
                return BadRequest(new { Message = "Invalid city selected." });
            }

            // Criar o novo utilizador
            user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Username,
                UserName = model.Username,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                CityId = model.CityId
            };

            // Upload da imagem, se fornecida
            if (model.ImageFile != null)
            {
                try
                {
                    var imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    user.ImageId = imageId;
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = $"Error uploading image: {ex.Message}" });
                }
            }

            // Adicionar o utilizador
            var result = await _userRepository.AddUserAsync(user, model.Password);
            if (result != IdentityResult.Success)
            {
                return BadRequest(new { Message = "The user couldn't be created.", Errors = result.Errors });
            }

            // Atribuir o role ao utilizador
            await _userHelper.AddUserToRoleAsync(user, "Customer");

            // Gerar token de confirmação de email
            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

            string tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            var response = _mailHelper.SendEmail(model.Username, "AirCinel - Confirm your Email",
                $"<h1 style=\"color:#1E90FF;\">Welcome to AirCinel!</h1>" +
                $"<p>To complete your registration, please confirm your email address by clicking the link below:</p>" +
                $"<p><a href = \"{tokenLink}\" style=\"color:#FFA500; font-weight:bold;\">Confirm Email</a></p>"
            );

            if (!response.IsSuccess)
            {
                return StatusCode(500, "The registration email couldn't be sent.");
            }

            return Ok(new { Message = "User registered successfully. Please confirm your email." });
        }



        [HttpPost("uploadImage")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty or not provided.");
            }

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new { Message = "Unauthorized access." });
            }

            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            try
            {
                var imageId = await _blobHelper.UploadBlobAsync(file, "users");
                user.ImageId = imageId;

                var result = await _userRepository.UpdateUserAsync(user);
                if (!result.Succeeded)
                {
                    return StatusCode(500, new { Message = "Failed to update user profile picture." });
                }

                return Ok(user.ImageFullPath);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpPut("updateuser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new { Message = "Unauthorized access." });
            }

            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
            user.Address = model.Address ?? user.Address;

            try
            {
                var result = await _userRepository.UpdateUserAsync(user);
                if (!result.Succeeded)
                {
                    return StatusCode(500, new { Message = "Failed to update user information." });
                }

                return Ok(new { Message = "User information updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("getuser")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new { Message = "Unauthorized access." });
            }

            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var userDto = new
            {
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.Address
            };

            return Ok(userDto);
        }

    }
}