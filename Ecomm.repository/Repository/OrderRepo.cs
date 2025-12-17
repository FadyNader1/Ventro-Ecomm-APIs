using Ecomm.core.Entities.OrderEntities;
using Ecomm.core.Exceptions;
using Ecomm.core.Interfaces;
using Ecomm.core.Specification;
using Ecomm.repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.repository.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly DBContext context;

        public OrderRepo(IUnitOfWork unitOfWork,DBContext context)
        {
            this.unitOfWork = unitOfWork;
            this.context = context;
        }
        public async Task<bool> CreateOrderAsync(Order item)
        {
            try
            {
                await unitOfWork.Repository<Order>().AddAsync(item);
                await unitOfWork.CompleteAsync();
                return true;

            }
            catch (Exception)
            {
                return false;

            }

        }

        public  Task<Order?> GetOrderByIdAsync(int id,string buyerEmail)
        {
           var order= context.Set<Order>().Where(x=>x.BuyerEmail==buyerEmail&& x.Id==id)
                .Include(x=>x.OrderItems).Include(x=>x.DeliveryMethod).FirstOrDefaultAsync();
            if(order==null)
                throw new NotFoundException("Order not found, please check your order id and try again");

            return order;

        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orders = await context.Set<Order>().Where(x => x.BuyerEmail == buyerEmail)
                .Include(x => x.OrderItems).Include(x => x.DeliveryMethod).ToListAsync();

            if (orders == null)
                throw new NotFoundException("Orders not found for this user, please check your email and try again");
            return orders;

        }
    }
}
