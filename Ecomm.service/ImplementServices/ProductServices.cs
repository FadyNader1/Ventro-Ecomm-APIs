using AutoMapper;
using Ecomm.core.Entities;
using Ecomm.core.Exceptions;
using Ecomm.core.Interfaces;
using Ecomm.core.Specification;
using Ecomm.service.InterfaceServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Ecomm.service.ImplementServices
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepo productRepo;
        private readonly IUnitOfWork unitOfWork;

        public ProductServices(IProductRepo productRepo, IUnitOfWork unitOfWork)
        {
            this.productRepo = productRepo;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Product> AddProductAsync(Product product, List<string> photos)
        {
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(product.CategoryId);
            if (category == null)
                throw new NotFoundException("Category not found, invalid category id");
            var productadded = await productRepo.AddProductAsync(product);

            var images = photos.Select(x => new Photo()
            {
                ImageName = x,
                ProductId = product.Id
            }).ToList();

            await unitOfWork.Repository<Photo>().AddRangeAsync(images);
            await unitOfWork.CompleteAsync();

            if (productadded)
            {
                var spec = new ProductSpecification(product.Id);
                var pro = await unitOfWork.Repository<Product>().GetBySpecAsync(spec);
                return product;
            }
            throw new BadRequestException("Product not added, please try again");
        }

        public async Task<Product> DeleteProductAsync(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Id invalid");
            var spec = new ProductSpecification(id);
            var product = await unitOfWork.Repository<Product>().GetBySpecAsync(spec);
            if (product == null)
                throw new NotFoundException("product not found");
            var deleteproduct = await productRepo.DeleteProductAsync(product);
            if (deleteproduct)
                return product;
            throw new BadRequestException("Product not deleted, please try again");
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
            var products = await productRepo.GetAllProductsAsync();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var spec = new ProductSpecification(id);
            var product = await unitOfWork.Repository<Product>().GetBySpecAsync(spec);
            if (product == null)
                throw new NotFoundException(" product not found,invalid id");
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product, List<string> photos)
        {
            //delete old images from database
            var oldimages = product.Photos.ToList();
            unitOfWork.Repository<Photo>().DeleteRangeAsync(oldimages);
            await unitOfWork.CompleteAsync();


            foreach (var photo in photos)
            {
                product.Photos.Add(new Photo()
                {
                    ImageName = photo,
                    ProductId = product.Id
                });
            }

            var updateproduct = await productRepo.UpdateProductAsync(product);
            if (updateproduct)
                return product;

            throw new BadRequestException("product not updated, please try again");

        }
    }
}