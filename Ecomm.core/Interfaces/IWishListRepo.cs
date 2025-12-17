using Ecomm.core.Entities.WishListEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Interfaces
{
    public interface IWishListRepo
    {
        Task<WishList> AddToWishList(string userId, int productId);
        Task<WishList> RemoveFromWishList(string userId, int productId);
        Task<IReadOnlyList<WishList>> GetWishListByUserId( string userId);
        Task<bool> ProductInWishListAsync(int productId, string userId);
    }
}
