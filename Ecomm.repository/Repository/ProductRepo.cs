using Ecomm.core.Entities;
using Ecomm.core.Interfaces;
using Ecomm.core.Specification;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.repository.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<ProductRepo> logger;

        public ProductRepo(IUnitOfWork unitOfWork, ILogger<ProductRepo> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task<bool> AddProductAsync(Product product)
        {
            try
            {
                await unitOfWork.Repository<Product>().AddAsync(product);
                await unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while adding product");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(Product product)
        {
            try
            {
                unitOfWork.Repository<Product>().Delete(product);
                await unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while deleting product");
                return false;
            }
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
            try
            {
                var spec = new ProductSpecification();
                var products = await unitOfWork.Repository<Product>().GetAllSpecAsync(spec);
                return products;

            }catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while getting all products");
                return new List<Product>();
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                var spec = new ProductSpecification(id);
                return await unitOfWork.Repository<Product>().GetBySpecAsync(spec);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while getting product by id");
                return null;
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                unitOfWork.Repository<Product>().Update(product);
                await unitOfWork.CompleteAsync();
                return true;
            }catch(Exception ex)
                {
                    logger.LogError(ex.InnerException, $"Error occured while updating product=> {ex.InnerException}");
                    return false;
                }
        }
    }
}
