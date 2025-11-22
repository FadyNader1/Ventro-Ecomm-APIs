using Ecomm.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Specification
{
    public class ProductCountSpecification:BaseSpecification<Product>
    {
        public ProductCountSpecification(ProductParams? productParams)
        {
            if (!string.IsNullOrEmpty(productParams?.SearchByName))
                Criteria = x => x.Name.ToLower().Contains(productParams.SearchByName.ToLower());

            if (productParams?.CategoryId.HasValue == true)
                Criteria = x => x.CategoryId == productParams.CategoryId;
        }
      
    }
}
