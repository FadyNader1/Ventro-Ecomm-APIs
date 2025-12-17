using Ecomm.core.DTOs.Order;
using Ecomm.core.Entities.OrderEntities;
using Ecomm.core.Exceptions;
using Ecomm.core.Interfaces;
using Ecomm.core.Specification;
using Ecomm.service.InterfaceServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.ImplementServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo orderRepo;
        private readonly ICustomerBasket customerBasket;
        private readonly IDeliveryMethodService deliveryMethodService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public OrderService(IOrderRepo orderRepo, ICustomerBasket customerBasket,IDeliveryMethodService deliveryMethodService,IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            this.orderRepo = orderRepo;
            this.customerBasket = customerBasket;
            this.deliveryMethodService = deliveryMethodService;
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }
        public async Task<Order> CreateOrder(OrderDto orderdto, string EmailBuyer)
        {
            var basket = await customerBasket.GetCustomerBasket(orderdto.BasketId);
            if(basket == null)
                throw new BadRequestException("Basket is empty");
            var orderItem =new  List<OrderItem>();
            foreach (var item in basket.Items)
            {
               var Item = new OrderItem()
                {
                    ProductId = item.ProductId,
                    Price = item.NewPrice,
                    Quantity = item.Quantity,
                    ProductName = item.ProductName,
                    MainImageUrl = $"{configuration["baseUrl"]}/{item.Photos[0]}",
                    Description = item.Description,
                    
                    
                };
                orderItem.Add(Item);
            }
            var deliveryMethod = await deliveryMethodService.GetDeliveryMethodByIdAsync(orderdto.DeleiveryMethodId);
            var subtotal = orderItem.Sum(item => item.Price * item.Quantity);
            var shipping = new ShippingAddress()
            {
                FullName = orderdto.shippingAddress.FullName,
                Country = orderdto.shippingAddress.Country,
                City = orderdto.shippingAddress.City,
                PostalCode = orderdto.shippingAddress.PostalCode,
                Street = orderdto.shippingAddress.Street
            };
            var order = new Order()
            {
                BuyerEmail = EmailBuyer,
                OrderItems = orderItem,
                shippingAddress = shipping,
                DeliveryMethodId = orderdto.DeleiveryMethodId,
                OrderDate = DateTime.UtcNow,
                Subtotal = subtotal,
                PaymentIntentId = basket.PaymentIntentId,
                Status = OrderStatus.Pending,
                

            };

           var createdOrder= await orderRepo.CreateOrderAsync(order);
            if (!createdOrder)
                throw new BadRequestException("Order not created");
            var orderSpec = new OrderSpecification(order.Id);
            var orderResult = await unitOfWork.Repository<Order>().GetBySpecAsync(orderSpec);
            return orderResult;


        }

      

        public async Task<Order> GetOrderById(int id, string buyerEmail)
        {
            var order=await orderRepo.GetOrderByIdAsync(id, buyerEmail);
            if (order == null)
                throw new NotFoundException("Order not found, please check your order id");

            return order;

        }
        
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orders=await orderRepo.GetOrdersForUserAsync(buyerEmail);
            if (orders == null || orders.Count == 0)
                throw new NotFoundException("Orders not found for this user, please check your email and try again");
            return orders;
        }
    }
}
