using Ecomm.core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.DTOs.Order
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public ShippingAddressDto shippingAddress { get; set; }
        public int DeleiveryMethodId { get; set; }

    }
    public class ShippingAddressDto
    {
        public string FullName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}
