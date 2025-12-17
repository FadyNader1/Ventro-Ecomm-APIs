using Ecomm.core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.DTOs.Basket
{
    public class AddToBasketDto
    {
        public string BasketId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public int? DeliveryMethodId { get; set; }
        public ShippingAddress? shippingAddress { get; set; }
        public string? EmailBuyer { get; set; }  
        public decimal? ShipPrice { get; set; }
    }
}
