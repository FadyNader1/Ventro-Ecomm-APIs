using Ecomm.core.Entities.WishListEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Specification
{
    public class WishListSpecification:BaseSpecification<WishList>
    {
        public WishListSpecification(string appUserId)
        {
            this.Criteria = x => x.AppUserId == appUserId;
            Includes.Add(x => x.Product);
            Includes.Add(x => x.Product.Category);
            Includes.Add(x => x.Product.Photos);

        }
    }
}
