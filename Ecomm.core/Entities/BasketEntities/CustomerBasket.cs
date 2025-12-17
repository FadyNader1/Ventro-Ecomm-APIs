using Ecomm.core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Entities.BasketEntities
{
    public class CustomerBasket
    {
        public string BasketId { get; set; }
        public List<ItemBasket> Items { get; set; } = new List<ItemBasket>();
        public string? EmailBuyer { get; set; }
        public ShippingAddress? shippingAddress { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }

        public CustomerBasket(string id)
        {
            this.BasketId = id;
        }
        public CustomerBasket()
        {
            
        }
    }
}
