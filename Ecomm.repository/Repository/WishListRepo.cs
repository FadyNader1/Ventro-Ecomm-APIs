using Ecomm.core.Entities.WishListEntities;
using Ecomm.core.Exceptions;
using Ecomm.core.Interfaces;
using Ecomm.core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.repository.Repository
{
    public class WishListRepo : IWishListRepo
    {
        private readonly IUnitOfWork unitOfWork;

        public WishListRepo(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<WishList> AddToWishList(string userId, int productId)
        {
            var sepc = new WishListByUserAndProductSpec(userId, productId);
            var CheckProductInWishList = await unitOfWork.Repository<WishList>().GetBySpecAsync(sepc);
            if (CheckProductInWishList != null)
                throw new BadRequestException("Product already in wish list");
            var wishList = new WishList()
            {
                AppUserId = userId,
                ProductId = productId
            };

          await unitOfWork.Repository<WishList>().AddAsync(wishList);
            await unitOfWork.CompleteAsync();
            return wishList;
        }

        public async Task<IReadOnlyList<WishList>> GetWishListByUserId(string userId)
        {
            var spec = new WishListSpecification(userId);
            var wishList = await unitOfWork.Repository<WishList>().GetAllSpecAsync(spec);
            return wishList;
        }

        public async Task<bool> ProductInWishListAsync(int productId,string userId)
        {
            var spec = new WishListByUserAndProductSpec(userId, productId);
            var wishList =await unitOfWork.Repository<WishList>().GetBySpecAsync(spec);
            if (wishList == null)
                return false;
            return true;
        }

        public async Task<WishList> RemoveFromWishList(string userId, int productId)
        {
            var sepc = new WishListByUserAndProductSpec(userId, productId);
            var CheckProductInWishList = await unitOfWork.Repository<WishList>().GetBySpecAsync(sepc);
            if (CheckProductInWishList == null)
                throw new BadRequestException("Product not in wish list");

            unitOfWork.Repository<WishList>().Delete(CheckProductInWishList);
            await unitOfWork.CompleteAsync();
            return CheckProductInWishList;
        }   
    }
}
