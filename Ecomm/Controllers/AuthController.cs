using Ecomm.core.DTOs.Identity;
using Ecomm.core.Exceptions;
using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecomm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuth auth;
        private readonly IConfiguration configuration;

        public AuthController(IAuth auth, IConfiguration configuration)
        {
            this.auth = auth;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponseAuth>> Register(RegisterDto registerDto)
        {
            var result = await auth.RegisterAsync(registerDto);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponseAuth>> Login(LoginDto loginDto)
        {
            var result = await auth.LoginAsync(loginDto);
            return Ok(result);
        }

        [HttpPost("confirm-email")]
        public async Task<ActionResult<string>> ConfirmEmail([FromQuery] string token, [FromQuery] string userId)
        {
            var result = await auth.ConfirmEmailAsync(token, userId);

            return Ok(new { message = result });
        }


        [Authorize]
        [HttpPatch("change-password")]
        public async Task<ActionResult<string>> ChangePassword(string CurrentPassword, string NewPassword)
        {
            var result = await auth.ChangePasswordAsync(CurrentPassword, NewPassword);
            return Ok(new { message = result });
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponseAuth>> RefreshToken()
        {
            // 1. هنا بنمسك الـ refresh token من الكوكيز يدوياً
            var refreshTokenFromCookie = Request.Cookies["RefreshToken"];

            // 2. التحقق لو الكوكي مش موجودة أصلاً
            if (string.IsNullOrEmpty(refreshTokenFromCookie))
            {
                return Unauthorized(new { message = "Refresh token is missing from cookies" });
            }

            // 3. بنبعت التوكن اللي مسكناه للدالة بتاعتك زي ما هي بدون أي تغيير في الـ Service
            var result = await auth.RefreshTokenAsync(refreshTokenFromCookie);

            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            var result = await auth.ForgotPasswordAsync(email);
            return Ok(new { message = result });

        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string token, string newPassword)
        {
            var result = await auth.ResetPasswordAsync(email, token, newPassword);
            return Ok(new { message = result });
        }

        [HttpPost("resend-confirm-email")]
        public async Task<IActionResult> ResendConfirmEmail([FromQuery] string email)
        {
            var result = await auth.ResendConfirmEmailAsync(email);
            return Ok(new { message = result });
        }
        [Authorize]
        [HttpGet("get-current-user")]
        public async Task<ActionResult<ApiResponseAuth>> GetUser()
        {
            var result = await auth.CurrentUserAsync();
            return Ok(result);
        }

        [HttpGet("signin-google")]
        public ActionResult SiginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var propertie = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(propertie, "Google");
        }
        [HttpGet("google-response")]
        public async Task<ActionResult> GoogleResponse()
        {
            var result = await auth.GoogleResponseAsync();
            return Redirect($"http://localhost:4200/google-success?token={result.Token}&refreshToken={result.RefreshToken}");
        }

        [HttpGet("signin-facebook")]
        public ActionResult SiginWithFacebook()
        {
            var redirectUrl = Url.Action("FacebookResponse", "Auth");
            var propertie = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(propertie, "Facebook");
        }

        [HttpGet("facebook-response")]
        public async Task<ActionResult> FacebookResponse()
        {
            var result = await auth.FacebookResponseAsync();
            return Redirect($"http://localhost:4200/facebook-success?token={result.Token}&refreshToken={result.RefreshToken}");
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponseAuth>> Logout()
        {
            var result = await auth.LogoutAsync();
            return Ok(result);
        }
    }
}