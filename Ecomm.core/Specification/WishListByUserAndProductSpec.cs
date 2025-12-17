using Ecomm.core.Entities.WishListEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Specification
{
    public class WishListByUserAndProductSpec:BaseSpecification<WishList>
    {
        public WishListByUserAndProductSpec(string UserId,int ProductId)
        {
            this.Criteria = x => x.AppUser.Id == UserId && x.Product.Id == ProductId;
        }
      
    }
}
