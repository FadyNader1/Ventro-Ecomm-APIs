using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Entities.OrderEntities
{
    public class DeliveryMethod:BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int  DeleveryDays { get; set; }
        public string Description { get; set; }

    }
}
