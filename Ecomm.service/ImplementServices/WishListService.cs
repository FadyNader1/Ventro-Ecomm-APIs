using Ecomm.core.Entities.IdentityModle;
using Ecomm.core.Entities.WishListEntities;
using Ecomm.core.Exceptions;
using Ecomm.core.Interfaces;
using Ecomm.core.Specification;
using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.ImplementServices
{
    public class WishListService : IWishListService
    {
        private readonly IWishListRepo wishListRepo;
        private readonly UserManager<AppUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IProductRepo productRepo;


        public WishListService(IWishListRepo wishListRepo,UserManager<AppUser> userManager,IHttpContextAccessor httpContextAccessor,IProductRepo productRepo)
        {
            this.wishListRepo = wishListRepo;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.productRepo = productRepo;
        }
        private HttpContext httpContext=> httpContextAccessor.HttpContext!;
        public async Task<WishList> AddToWishListAsync( int productId)
        {
            var email= httpContext.User.FindFirstValue(ClaimTypes.Email);
            if (email == null) 
                throw new UnAuthorizeException("User is not authorized");
            var user=await userManager.FindByEmailAsync(email);
            if(user==null) 
                throw new NotFoundException("User not found");
            var product=await productRepo.GetProductByIdAsync(productId);
            if(product==null) 
                throw new NotFoundException("Product not found");

            return await wishListRepo.AddToWishList(user.Id, productId);

        }

        public async Task<IReadOnlyList<WishList>> GetWishListByUserIdAsync()
        {
            var email = httpContext.User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                throw new UnAuthorizeException("User is not authorized");
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NotFoundException("User not found");
            return await wishListRepo.GetWishListByUserId(user.Id);
        }

        public async Task<bool> IsProductInWishListAsync(int productId)
        {
            var email = httpContext.User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                throw new UnAuthorizeException("User is not authorized");
            var user =await  userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NotFoundException("User not found");

            var wishList = await wishListRepo.ProductInWishListAsync(productId,user.Id);
            return wishList;


        }

        public async Task<WishList> RemoveFromWishListAsync( int productId)
        {
            var email = httpContext.User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                throw new UnAuthorizeException("User is not authorized");

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NotFoundException("User not found");
            var product = await productRepo.GetProductByIdAsync(productId);
            if (product == null)
                throw new NotFoundException("Product not found");
          
               return await wishListRepo.RemoveFromWishList(user.Id, productId);
        }
    }
}
