using Ecomm.core.Entities.WishListEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.InterfaceServices
{
    public interface IWishListService
    {
        Task<WishList> AddToWishListAsync(int productId);
        Task<WishList> RemoveFromWishListAsync(int productId);
        Task<IReadOnlyList<WishList>> GetWishListByUserIdAsync();
        Task<bool> IsProductInWishListAsync(int productId);
    }
}
