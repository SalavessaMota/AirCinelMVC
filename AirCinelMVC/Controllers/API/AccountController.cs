using AirCinelMVC.Data;
using AirCinelMVC.Data.Dtos;
using AirCinelMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AirCinelMVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;

        public AccountController(
            IUserRepository userRepository,
            IUserHelper userHelper,
            IMailHelper mailHelper)
        {
            _userRepository = userRepository;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
        }

        [HttpPost("recoverpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordDto model)
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
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
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
    }
}