using Ecomm.core.Entities;
using Ecomm.core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.InterfaceServices
{
    public interface IProductServices
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync(ProductParams? productParams);
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product, List<string> photos);
        Task<Product> UpdateProductAsync(Product product, List<string> photos);
        Task<Product> DeleteProductAsync(int id);
        Task<IReadOnlyList<Product>> ListAsync(ISpecification<Product> spec);
    }
}
