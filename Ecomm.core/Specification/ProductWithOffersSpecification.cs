using Ecomm.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Specification
{
    public class ProductWithOffersSpecification:BaseSpecification<Product>
    {
        public ProductWithOffersSpecification():base(x =>x.InventoryQuantity >0 && x.NewPrice<x.OldPrice)
        {
            AddOrderByDes(p => (p.OldPrice - p.NewPrice)); // ترتيب حسب أكبر خصم
            ApplyPaging(0, 8);
            Includes.Add(p => p.Photos);
            Includes.Add(p => p.Category);
        }
    }
}
