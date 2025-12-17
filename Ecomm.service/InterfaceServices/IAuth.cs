using Ecomm.core.DTOs.Identity;
using Ecomm.core.Entities.IdentityModle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.InterfaceServices
{
    public interface IAuth
    {
        public Task<ApiResponseAuth> RegisterAsync(RegisterDto registerDto);
        public Task<ApiResponseAuth> LoginAsync(LoginDto loginDto);
        public Task<string> ConfirmEmailAsync(string token, string UserId);
        public Task<string> ChangePasswordAsync(string CurrentPassword, string NewPassword);
        public Task<ApiResponseAuth> RefreshTokenAsync(string refreshToken);
        public Task<string> ForgotPasswordAsync(string email);  
        public Task<string> ResetPasswordAsync(string email, string token, string newPassword);
        public Task<string> ResendConfirmEmailAsync(string email);
        public Task<ApiResponseAuth> CurrentUserAsync();
        public Task<ApiResponseAuth> GoogleResponseAsync();
        public Task<ApiResponseAuth> FacebookResponseAsync();
        public Task<ApiResponseAuth> LogoutAsync();


    }
}
