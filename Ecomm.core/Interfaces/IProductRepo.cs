using Ecomm.core.Entities;
using Ecomm.core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Interfaces
{
    public interface IProductRepo
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync(ProductParams productParams);
        Task<Product?> GetProductByIdAsync(int id);
        Task<bool> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(Product product);
    }
}
