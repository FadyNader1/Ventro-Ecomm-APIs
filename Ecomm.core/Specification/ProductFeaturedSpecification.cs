using Ecomm.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Specification
{
    public class ProductFeaturedSpecification:BaseSpecification<Product>
    {
        public ProductFeaturedSpecification():base(x=>x.IsFeatured && x.InventoryQuantity >0)
        {
            
            ApplyPaging(0,8);
            Includes.Add(x => x.Category);
            Includes.Add(x => x.Photos);

        }
    }
}
