using Ecomm.core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Interfaces
{
    public interface IOrderRepo
    {
        Task<bool> CreateOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(int id, string buyerEmail);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
    }
}
