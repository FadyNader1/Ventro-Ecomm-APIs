using Ecomm.core.DTOs.Order;
using Ecomm.core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.InterfaceServices
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(OrderDto order,string EmailBuyer);
        Task<Order> GetOrderById(int id, string buyerEmail);
                Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);



    }
}
