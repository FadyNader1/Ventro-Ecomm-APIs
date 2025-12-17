using Ecomm.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Specification
{
    public class ProductLatestSpecification:BaseSpecification<Product>
    {
        public ProductLatestSpecification() : base(x => x.InventoryQuantity > 0)
        {

            AddOrderByDes(x => x.CreatedAt);
            ApplyPaging(0,8);
            Includes.Add(x => x.Category);
            Includes.Add(x => x.Photos);


        }
    }
}
