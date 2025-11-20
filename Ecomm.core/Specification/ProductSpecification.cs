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
        public ProductSpecification()
        {
            Includes.Add(x => x.Category);
            Includes.Add(x => x.Photos);

        }
        public ProductSpecification(int id):base(x=>x.Id==id)
        {
            Includes.Add(x => x.Category);
            Includes.Add(x => x.Photos);
        }

    }
}
