using Azure;
using Ecomm.core.DTOs.Identity;
using Ecomm.core.Entities;
using Ecomm.core.Entities.IdentityModle;
using Ecomm.core.Exceptions;
using Ecomm.core.Interfaces;
using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.ImplementServices
{
    public class Auth : IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IEmailSetting emailSetting;
        private readonly IConfiguration configuration;
        private readonly ITokenSetting tokenSetting;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRefreshTokenRepo refreshTokenRepo;
        private readonly IUnitOfWork unitOfWork;
        private readonly HttpContext httpContext;



        public Auth(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor, IEmailSetting emailSetting, IConfiguration configuration, ITokenSetting tokenSetting, RoleManager<IdentityRole> roleManager, IRefreshTokenRepo refreshTokenRepo, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSetting = emailSetting;
            this.configuration = configuration;
            this.tokenSetting = tokenSetting;
            this.roleManager = roleManager;
            this.refreshTokenRepo = refreshTokenRepo;
            this.unitOfWork = unitOfWork;
            this.httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<ApiResponseAuth> RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto == null)
                throw new BadRequestException("RegisterDto is null");

            var existUser = await userManager.FindByEmailAsync(registerDto.Email);
            if (existUser != null)
                throw new BadRequestException("Email already exist");

            var user = new AppUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException(errors);
            }
            //Assign Role
            if (!await roleManager.RoleExistsAsync("Customer"))
                await roleManager.CreateAsync(new IdentityRole("Customer"));

            if (!await userManager.IsInRoleAsync(user, "Customer"))
                await userManager.AddToRoleAsync(user, "Customer");
            //Confirm email
            var TokenForConfirmEmail = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var token = WebUtility.UrlEncode(TokenForConfirmEmail); // أفضل من Uri.EscapeDataString
            var userId = WebUtility.UrlEncode(user.Id);
            var confirmEmailUrl = $"{configuration["frontendUrl"]}confirm-email?token={token}&userId={userId}";
            Console.WriteLine(confirmEmailUrl);
            var sub = "Confirm your email address";
            var htmlbody = $@"
<body style=""margin:0;padding:0;background-color:#eef1ff;font-family:'Segoe UI', Arial, sans-serif;"">
    <div style=""max-width:520px;margin:50px auto;background:#ffffff;border-radius:14px;
                box-shadow:0 4px 12px rgba(0,0,0,0.08);text-align:center;padding:32px 24px;"">
                     
        <h1 style=""color:#4b5cff;margin-bottom:12px;font-size:24px;"">
            Welcome, {user.FirstName} {user.LastName}! 
        </h1>        
                     
        <p style=""color:#555;font-size:15px;margin:0 0 22px;line-height:1.6;"">
            Thank you for registering with us!  
            To activate your account and start exploring our store, please confirm your email address.
        </p>         
                     
        <a href=""{confirmEmailUrl}""
           style=""display:inline-block;padding:12px 26px;background:#4b5cff;
                  color:#ffffff;text-decoration:none;border-radius:8px;font-weight:600;
                  font-size:15px;margin-top:10px;"">
            Confirm Email
        </a>         
                     
        <p style=""color:#777;font-size:13px;margin-top:28px;"">
            If you didn’t create this account, you can safely ignore this email.
        </p>         
    </div>           
</body>";            
            await emailSetting.SendEmailAsync(sub, htmlbody, user.Email);

            var response = new ApiResponseAuth()
            {
                Message = "Register Success, please check your email to confirm your account",
                DisplayName = user.FirstName,
                Email = user.Email

            };
            return response;
        }

        public async Task<ApiResponseAuth> LoginAsync(LoginDto loginDto)
        {
            if (loginDto == null)
                throw new BadRequestException("LoginDto is null");

            var existUser = await userManager.FindByEmailAsync(loginDto.Email);
            if (existUser == null)
                throw new BadRequestException("Email not found");

            if (!await userManager.IsEmailConfirmedAsync(existUser))
                throw new BadRequestException("Email is not confirmed,please check your email to confirm your account");

            var checkPassword = await signInManager.CheckPasswordSignInAsync(existUser, loginDto.Password, true);
            if (!checkPassword.Succeeded)
                throw new BadRequestException("Password is incorrect");

            var response = new ApiResponseAuth()
            {
                Message = "Login Success",
                DisplayName = existUser.FirstName,
                Email = existUser.Email!,
                Token = await tokenSetting.CreateJwtTokenAsync(existUser, userManager),
                RefreshToken = await tokenSetting.CreateRefreshTokenAsync(existUser)

            };

            //add token in cookie
            httpContext.Response.Cookies.Append("Token", response.Token, new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(30),
                SameSite = !httpContext.Request.IsHttps ? SameSiteMode.Lax : SameSiteMode.None,
                Secure = !httpContext.Request.IsHttps ? false : true,
            });
            httpContext.Response.Cookies.Append("RefreshToken", response.RefreshToken, new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(30),
                SameSite = !httpContext.Request.IsHttps ? SameSiteMode.Lax : SameSiteMode.None,
                Secure = !httpContext.Request.IsHttps ? false : true,
            });

            return response;
        }

        public async Task<string> ConfirmEmailAsync([FromQuery] string token,[FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
                throw new BadRequestException("Token or UserId is null");
            Console.WriteLine(userId);
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                throw new BadRequestException("User not found");

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                throw new BadRequestException(string.Join(",", errors));
            }

            return "Confirm email success, please login";
        }

        public async Task<string> ChangePasswordAsync(string CurrentPassword, string NewPassword)
        {
            var email = httpContext.User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                throw new UnAuthorizeException(" Email not found, please login again");
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new BadHttpRequestException("user not found ");
            var CheckPassword = await userManager.CheckPasswordAsync(user, CurrentPassword);
            if (!CheckPassword)
                throw new BadRequestException("Invalid current password");
            var result = await userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                throw new BadRequestException(string.Join(",", errors));
            }

            return "Change password success";
        }

        public async Task<ApiResponseAuth> RefreshTokenAsync(string refreshToken)
        {
            // 1. التحقق من التوكن القديم
            var storedRefreshToken = await refreshTokenRepo.FindByToken(refreshToken);
            if (storedRefreshToken == null || storedRefreshToken.IsRevoked || storedRefreshToken.ExpiryDate <= DateTime.UtcNow)
                throw new UnAuthorizeException("Invalid or expired refresh token");

            var user = await userManager.FindByIdAsync(storedRefreshToken.UserId);

            // 2. إبطال التوكن القديم
            storedRefreshToken.IsRevoked = true;

            // 3. توليد توكنز جديدة
            var newAccessToken = await tokenSetting.CreateJwtTokenAsync(user, userManager);
            var newRefreshToken = await tokenSetting.CreateRefreshTokenAsync(user);

            // 4. !!! هـام: حفظ الـ Refresh Token الجديد في الداتابيز !!!
            var refTokenEntity = new RefreshToken()
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            };
            await unitOfWork.Repository<RefreshToken>().AddAsync(refTokenEntity);
            await unitOfWork.CompleteAsync();

            // 5. تحديث الكوكيز
            httpContext.Response.Cookies.Append("Token", newAccessToken, new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(30),
                SameSite = !httpContext.Request.IsHttps ? SameSiteMode.Lax : SameSiteMode.None,
                Secure = !httpContext.Request.IsHttps ? false : true,
            });
            httpContext.Response.Cookies.Append("RefreshToken", newRefreshToken, new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(30),
                SameSite = !httpContext.Request.IsHttps ? SameSiteMode.Lax : SameSiteMode.None,
                Secure = !httpContext.Request.IsHttps ? false : true,
            });


            return new ApiResponseAuth { Token = newAccessToken, RefreshToken = newRefreshToken };
        }
        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new BadRequestException("User not found");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var TokenBytes = Encoding.UTF8.GetBytes(token);
            var EncodedToken = WebEncoders.Base64UrlEncode(TokenBytes);

            var resetPasswordUrl = $"{configuration["frontendUrl"]}reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(EncodedToken)}";

            Console.WriteLine(resetPasswordUrl);

            var subject = "Reset Password";

            var htmlbody = @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>Reset Password</title>

    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', sans-serif;
            background: #f6f7fb;
        }

        .container {
            max-width: 520px;
            margin: 40px auto;
            background: #ffffff;
            border-radius: 18px;
            padding: 40px 35px;
            box-shadow: 0 8px 30px rgba(0,0,0,0.12);
            text-align: center;
        }

        .title {
            font-size: 26px;
            font-weight: 700;
            margin-bottom: 14px;
            color: #333;
        }

        .subtitle {
            font-size: 16px;
            color: #555;
            margin-bottom: 28px;
            line-height: 1.6;
        }

        .btn {
            display: inline-block;
            padding: 14px 28px;
            background: linear-gradient(135deg, #ff416c, #ff4b2b);
            color: #fff !important;
            text-decoration: none;
            border-radius: 10px;
            font-weight: 600;
            font-size: 16px;
            margin-top: 20px;
            transition: all .3s ease-in-out;
        }

        .btn:hover {
            opacity: .85;
            transform: translateY(-3px);
        }

        .footer {
            margin-top: 32px;
            font-size: 13px;
            color: #999;
        }

        .icon-circle {
            width: 85px;
            height: 85px;
            background: linear-gradient(135deg, #ff416c, #ff4b2b);
            border-radius: 50%;
            display: flex;
            justify-content: center;
            align-items: center;
            margin: 0 auto 20px;
            color: #fff;
            font-size: 40px;
            font-weight: bold;
        }
    </style>

</head>

<body>
    <div class='container'>
        <div class='icon-circle'>🔒</div>

        <h1 class='title'>Reset Your Password</h1>

        <p class='subtitle'>
            We received a request to reset your password.<br/>
            Click the button below to set a new password.
        </p>

        <a href='" + resetPasswordUrl + @"' class='btn'>Reset Password</a>

        <p class='footer'>
            If you didn’t request this, you can safely ignore this email.
        </p>
    </div>
</body>
</html>";

            await emailSetting.SendEmailAsync(subject, htmlbody, user.Email!);
            return "Please check your email to reset your password";
        }

        public async Task<string> ResetPasswordAsync(string email, string token, string newPassword)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPassword))
                throw new BadRequestException("Email, token, or new password is null");
            var user=await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new BadRequestException("User not found");

            var tokenBytes = WebEncoders.Base64UrlDecode(token);
            var tokenString = Encoding.UTF8.GetString(tokenBytes);

            var result=await userManager.ResetPasswordAsync(user, tokenString, newPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                throw new BadRequestException(string.Join(",", errors));
            }
            return "Reset password success, you can login with your new password";
        }

        public async Task<string> ResendConfirmEmailAsync(string email)
        {
            var user=await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new BadRequestException("User not found");

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var tokenString = WebEncoders.Base64UrlEncode(tokenBytes);
            var userId = WebUtility.UrlEncode(user.Id);
            var confirmEmailUrl = $"{configuration["frontendUrl"]}confirm-email?token={tokenString}&UserId={userId}";
            Console.WriteLine(confirmEmailUrl);
            var subject = "Confirm your email";
            var htmlbody = $@"
<body style=""margin:0;padding:0;background-color:#eef1ff;font-family:'Segoe UI', Arial, sans-serif;"">
    <div style=""max-width:520px;margin:50px auto;background:#ffffff;border-radius:14px;
                box-shadow:0 4px 12px rgba(0,0,0,0.08);text-align:center;padding:32px 24px;"">
                     
        <h1 style=""color:#4b5cff;margin-bottom:12px;font-size:24px;"">
            Welcome, {user.FirstName} {user.LastName}! 
        </h1>        
                     
        <p style=""color:#555;font-size:15px;margin:0 0 22px;line-height:1.6;"">
            Thank you for registering with us!  
            To activate your account and start exploring our store, please confirm your email address.
        </p>         
                     
        <a href=""{confirmEmailUrl}""
           style=""display:inline-block;padding:12px 26px;background:#4b5cff;
                  color:#ffffff;text-decoration:none;border-radius:8px;font-weight:600;
                  font-size:15px;margin-top:10px;"">
            Confirm Email
        </a>         
                     
        <p style=""color:#777;font-size:13px;margin-top:28px;"">
            If you didn’t create this account, you can safely ignore this email.
        </p>         
    </div>           
</body>";

            await emailSetting.SendEmailAsync(subject, htmlbody, user.Email!);
            return "Please check your email to confirm your account";

        }

        public async Task<ApiResponseAuth> GoogleResponseAsync()
        {
            var result = await httpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var cliams = result.Principal!.Identities.FirstOrDefault()!.Claims;
            var email = cliams.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name= cliams.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var user=await userManager.FindByEmailAsync(email!);
            if (user == null)
            {
                var newUser = new AppUser()
                {
                    FirstName = name?.Split(' ').FirstOrDefault() ?? "GoogleUser",
                    LastName = name?.Split(' ').Skip(1).FirstOrDefault() ?? "GoogleUser",
                    Email = email!,
                    UserName = email!.Split('@')[0],
                    EmailConfirmed = true


                };
                var createResult = await userManager.CreateAsync(newUser);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new BadRequestException(errors);
                }
               
                user = newUser;
            }
            //Assign Role
            if (!await roleManager.RoleExistsAsync("Customer"))
                await roleManager.CreateAsync(new IdentityRole("Customer"));

            if (!await userManager.IsInRoleAsync(user, "Customer"))
                await userManager.AddToRoleAsync(user, "Customer");

            var response=new ApiResponseAuth()
            {
                Message = "Login with Google Success",
                DisplayName = user.FirstName,
                Email = user.Email!,
                Token = await tokenSetting.CreateJwtTokenAsync(user, userManager),
                RefreshToken = await tokenSetting.CreateRefreshTokenAsync(user)
            };
            
            //add token in cookie
            httpContext.Response.Cookies.Append("Token", response.Token, new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(30),
                SameSite = !httpContext.Request.IsHttps ? SameSiteMode.Lax : SameSiteMode.None,
                Secure = !httpContext.Request.IsHttps ? false : true,
            });
            httpContext.Response.Cookies.Append("RefreshToken", response.RefreshToken, new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(30),
                SameSite = !httpContext.Request.IsHttps ? SameSiteMode.Lax : SameSiteMode.None,
                Secure = !httpContext.Request.IsHttps ? false : true,
            });


            var refToken = new RefreshToken()
            {
                UserId = user.Id,
                Token = response.RefreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                IsRevoked = false

            };

            await unitOfWork.Repository<RefreshToken>().AddAsync(refToken);

            return response;
        }
        public async Task<ApiResponseAuth> CurrentUserAsync()
        {
            var email = httpContext.User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                throw new UnAuthorizeException(" Email not found, please login again");

            var user =await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new BadHttpRequestException("user not found ");

            return new ApiResponseAuth()
            {
                Message = "Current User",
                DisplayName = user.FirstName,
                Email = user.Email!,
                Token = await tokenSetting.CreateJwtTokenAsync(user, userManager),
                RefreshToken = await tokenSetting.CreateRefreshTokenAsync(user)
            };


        }

        public async Task<ApiResponseAuth> FacebookResponseAsync()
        {
            var result = await httpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            var cliams = result.Principal!.Identities.FirstOrDefault()!.Claims;
            var email = cliams.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = cliams.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var user = await userManager.FindByEmailAsync(email!);
            if (user == null)
            {
                var newUser = new AppUser()
                {
                    FirstName = name?.Split(' ').FirstOrDefault() ?? "FacebookUser",
                    LastName = name?.Split(' ').Skip(1).FirstOrDefault() ?? "FacebookUser",
                    Email = email!,
                    UserName = email!.Split('@')[0],
                    EmailConfirmed = true


                };
                var createResult = await userManager.CreateAsync(newUser);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new BadRequestException(errors);
                }
                user = newUser;
            }
            //Assign Role
            if (!await roleManager.RoleExistsAsync("Customer"))
                await roleManager.CreateAsync(new IdentityRole("Customer"));

            if (!await userManager.IsInRoleAsync(user!, "Customer"))
                await userManager.AddToRoleAsync(user!, "Customer");
            var response= new ApiResponseAuth()
            {
                Message = "Login with Facebook Success",
                DisplayName = user.FirstName,
                Email = user.Email!,
                Token = await tokenSetting.CreateJwtTokenAsync(user, userManager),
                RefreshToken = await tokenSetting.CreateRefreshTokenAsync(user)
            };
            //add token in cookie
            httpContext.Response.Cookies.Append("Token", response.Token, new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(30),
                SameSite = !httpContext.Request.IsHttps ? SameSiteMode.Lax : SameSiteMode.None,
                Secure = !httpContext.Request.IsHttps ? false : true,
            });
            httpContext.Response.Cookies.Append("RefreshToken", response.RefreshToken, new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(30),
                SameSite = !httpContext.Request.IsHttps ? SameSiteMode.Lax : SameSiteMode.None,
                Secure = !httpContext.Request.IsHttps ? false : true,
            });
            return response;
        }

        public async Task<ApiResponseAuth> LogoutAsync()
        {
            var email = httpContext.User.FindFirstValue(ClaimTypes.Email);
            if (email == null) throw new UnAuthorizeException("You are already logged out");

            var user = await userManager.FindByEmailAsync(email);
            await signInManager.SignOutAsync();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = !httpContext.Request.IsHttps ? false : true,
                Expires = DateTime.Now.AddDays(-1),
                SameSite = !httpContext.Request.IsHttps ? SameSiteMode.Lax : SameSiteMode.None,
                Path = "/"
            };

            foreach (var cookie in httpContext.Request.Cookies.Keys)
            {
                httpContext.Response.Cookies.Delete(cookie, cookieOptions);
            }

            httpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application", cookieOptions);

            return new ApiResponseAuth
            {
                Message = "Logout success",
                DisplayName = user!.FirstName,
                Email = user.Email!,
                Token = null,
                RefreshToken = null
            };
        }


    }
}