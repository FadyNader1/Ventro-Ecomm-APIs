using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Entities.OrderEntities
{
    public class Order:BaseEntity
    {
        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public ShippingAddress shippingAddress { get; set; }= default!;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public OrderStatus Status { get; set; }= OrderStatus.Pending;
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }=default!;
        public decimal Subtotal { get; set; }
        public decimal Total()
        {
            return Subtotal + DeliveryMethod.Price;
        }
        public string? PaymentIntentId { get; set; }
    }
}
