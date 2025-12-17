using Ecomm.core.Entities.IdentityModle;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.InterfaceServices
{
    public interface ITokenSetting
    {
        Task<string> CreateJwtTokenAsync(AppUser appUser, UserManager<AppUser> userManager);
        Task<string> CreateRefreshTokenAsync(AppUser appUser);

    }
}
