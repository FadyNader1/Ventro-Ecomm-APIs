using Ecomm.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Specification
{
    public class ProductSpecification:BaseSpecification<Product>
    {
        public ProductSpecification(ProductParams? productParams)
        {

            if (!string.IsNullOrEmpty(productParams?.Sort))
                switch (productParams?.Sort)
                {
                    case "priceAsc":
                        AddOrderByAsyn(x => x.NewPrice);
                        break;
                    case "priceDesc":
                        AddOrderByDes(x => x.NewPrice);
                        break;
                    default:
                        AddOrderByAsyn(x => x.Id);
                        break;
                }

            if (!string.IsNullOrEmpty(productParams?.SearchByName))
                Criteria = x => x.Name.ToLower().Contains(productParams.SearchByName.ToLower());

            if (productParams?.CategoryId.HasValue == true)
                Criteria = x => x.CategoryId == productParams.CategoryId;

            Includes.Add(x => x.Category);
            Includes.Add(x => x.Photos);

            ApplyPaging(productParams.PageSize*(productParams.PageIndex-1), productParams.PageSize);

        }
        public ProductSpecification(int id):base(x=>x.Id==id)
        {
            Includes.Add(x => x.Category);
            Includes.Add(x => x.Photos);
        }

    }
}
