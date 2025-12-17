using Ecomm.core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Specification
{
    public class OrderSpecification:BaseSpecification<Order>
    {
        public OrderSpecification()
        {
            Includes.Add(x => x.OrderItems);
            Includes.Add(x => x.DeliveryMethod);

        }
        public OrderSpecification(int id):base(x => x.Id == id)
        {
            Includes.Add(x => x.OrderItems);
            Includes.Add(x => x.DeliveryMethod);


        }
    }
}
