using Ecomm.core.Entities;
using Ecomm.core.Entities.IdentityModle;
using Ecomm.core.Interfaces;
using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.ImplementServices
{
    public class TokenSetting : ITokenSetting
    {
        private readonly IConfiguration configuration;
        private readonly IRefreshTokenRepo refreshTokenRepo;

        public TokenSetting(IConfiguration configuration,IRefreshTokenRepo refreshTokenRepo)
        {
            this.configuration = configuration;
            this.refreshTokenRepo = refreshTokenRepo;
        }
        public async Task<string> CreateJwtTokenAsync(AppUser appUser, UserManager<AppUser> userManager)
        {
            //authClaims
            var AuthClaims = new List<Claim>();
            AuthClaims.Add(new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()));
            AuthClaims.Add(new Claim(ClaimTypes.GivenName, appUser.FirstName));
            AuthClaims.Add(new Claim(ClaimTypes.Surname, appUser.LastName));
            AuthClaims.Add(new Claim(ClaimTypes.Email, appUser.Email!));
            //roles
            var userRoles = await userManager.GetRolesAsync(appUser);
            foreach (var userRole in userRoles)
                AuthClaims.Add(new Claim(ClaimTypes.Role, userRole));

            //security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));

            //signing credentials
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: AuthClaims,
                expires: DateTime.Now.AddMinutes(int.Parse(configuration["Jwt:ExpireTime"]!)),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            //create Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshTokenAsync(AppUser appUser)
        {
            var refreshToken = new RefreshToken()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = appUser.Id!,
                ExpiryDate = DateTime.Now.AddDays(30),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            await refreshTokenRepo.AddRefreshTokenAsync(refreshToken);
            return refreshToken.Token;
        }
    }
}
