using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecomm.core.Entities.OrderEntities
{
    public class OrderItem:BaseEntity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string MainImageUrl { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        [JsonIgnore]
        public Order order { get; set; }


    }
}
