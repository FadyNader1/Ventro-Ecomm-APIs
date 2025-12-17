using Ecomm.core.DTOs.Basket;
using Ecomm.core.Entities.BasketEntities;
using Ecomm.core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.InterfaceServices
{
    public interface ICustomerBasket
    {
        string CreateBasket();
        Task<CustomerBasket> AddItemToBasket(AddToBasketDto addToBasketDto);
        Task<CustomerBasket> GetCustomerBasket(string basketId);
        Task<bool> RemoveItemFromBasket(string basketId, int productId);
        Task<bool> ClearBasket(string basketId);
        Task<CustomerBasket> UpdateBasket(CustomerBasket basket);
    }
}
