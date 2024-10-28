using AirCinelMVC.Data;
using AirCinelMVC.Data.Dtos;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
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

        public AccountController(
            ICountryRepository countryRepository,
            IUserRepository userRepository,
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IBlobHelper blobHelper
            )
        {
            _countryRepository = countryRepository;
            _userRepository = userRepository;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _blobHelper = blobHelper;
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
                return BadRequest(new { Message = "Error changing password." });
            }

            return Ok(new { Message = "Password changed successfully." });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAPI([FromBody] RegisterDto model)
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

            user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Username,
                UserName = model.Username,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                CityId = model.CityId,
                ImageId = model.ImageId
            };

            if (model.ImageId != Guid.Empty)
            {
                var imageId = await _blobHelper.VerifyAndUploadImageAsync(model.ImageId, "users");
                user.ImageId = imageId; 
            }

            var result = await _userRepository.AddUserAsync(user, model.Password);
            if (result != IdentityResult.Success)
            {
                return BadRequest(new { Message = "The user couldn't be created.", Errors = result.Errors });
            }

            await _userHelper.AddUserToRoleAsync(user, "Customer");
            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

            string tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            var response = _mailHelper.SendEmail(model.Username, "AirCinel - Confirm your Email",
                $"<h1 style=\"color:#1E90FF;\">Welcome to AirCinel!</h1>" +
                $"<p>Thank you for choosing AirCinel, your trusted airline for premium travel experiences.</p>" +
                $"<p>To complete your registration, please confirm your email address by clicking the link below:</p>" +
                $"<p><a href = \"{tokenLink}\" style=\"color:#FFA500; font-weight:bold;\">Confirm Email</a></p>" +
                $"<p>If you didn’t create this account, please disregard this email.</p><br>" +
                $"<p>Safe travels,</p>" +
                $"<p>The AirCinel Team</p>" +
                $"<p><small>This is an automated message. Please do not reply to this email.</small></p>"
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

            try
            {
                var imageId = await _blobHelper.UploadBlobAsync(file, "users");
                return Ok(imageId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}