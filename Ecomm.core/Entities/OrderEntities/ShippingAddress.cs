using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Entities.OrderEntities
{
    public class ShippingAddress
    {
        public string FullName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }    
        public string Street { get; set; }    
        public string PostalCode { get; set; }    
    }
}
