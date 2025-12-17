using AutoMapper;
using Ecomm.core.Entities;
using Ecomm.core.Interfaces;
using Ecomm.core.Specification;
using Ecomm.DTOs.ProductDTOs;
using Ecomm.Helper;
using Ecomm.repository.Repository;
using Ecomm.Responses;
using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductServices productServices;
        private readonly IMapper mapper;
        private readonly ImageSetting imageSetting;
        private readonly IUnitOfWork unitOfWork;

        public ProductController(IProductServices productServices, IMapper mapper, ImageSetting imageSetting,IUnitOfWork unitOfWork)
        {
            this.productServices = productServices;
            this.mapper = mapper;
            this.imageSetting = imageSetting;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("getallproducts")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<ProductDto>>>> GetAllProducts([FromQuery] ProductParams productParams)
        {
            var countspec = new ProductCountSpecification(productParams);
            var count = await unitOfWork.Repository<Product>().GetCountSpecAsync(countspec);

            var products = await productServices.GetAllProductsAsync(productParams);
            var productsDto = mapper.Map<IReadOnlyList<ProductDto>>(products);
            var response = new ApiResponse<IReadOnlyList<ProductDto>>()
            {
                Success = true,
                Meta = new Meta()
                {
                    PageIndex = productParams.PageIndex,
                    PageSize = productParams.PageSize,
                    Count = count,
                    TotalPages = (int)Math.Ceiling((double)count / productParams.PageSize),
                    HasNextPage = productParams.PageIndex < ((int)Math.Ceiling((double)count / productParams.PageSize)),
                    HasPreviousPage = productParams.PageIndex > 1,
                },
                Message = "All products",
                Data = productsDto
            };

            return Ok(response);

        }

        [HttpPost("addproduct")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> AddProduct([FromBody]AddProductDto addProductDto)
        {
            var productmap = mapper.Map<Product>(addProductDto);
            var images = ImageSetting.saveImage(addProductDto.Photos, addProductDto.Name);


            var product = await productServices.AddProductAsync(productmap, images);
            var productDto = mapper.Map<ProductDto>(product);


            var response = new ApiResponse<ProductDto>()
            {
                Success = true,
                Message = "Product added successfully",
                Data = productDto
            };
            return Ok(response);
        }

        [HttpGet("getproductbyid/{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetProductById(int id)
        {
            var product = await productServices.GetProductByIdAsync(id);
            var productDto = mapper.Map<ProductDto>(product);
            var response = new ApiResponse<ProductDto>()
            {
                Success = true,
                Message = "Product fetched successfully",
                Data = productDto
            };
            return Ok(response);
        }
        [HttpDelete("deleteproduct/{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> DeleteProduct(int id)
        {
            var product = await productServices.DeleteProductAsync(id);
            if (product != null)
            {
                foreach (var photo in product.Photos)
                {
                    imageSetting.DeleteImage(photo.ImageName);
                }
            }

            var productdto = mapper.Map<ProductDto>(product);
            var response = new ApiResponse<ProductDto>()
            {
                Success = true,
                Message = "Product deleted successfully",
                Data = productdto
            };
            return Ok(response);
        }

        [HttpPut("updateproduct")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> UpdateProduct(UpdateProdcutDto updateProdcutDto)
        {
            //check if product exists
            var getproduct = await productServices.GetProductByIdAsync(updateProdcutDto.Id);
            if (getproduct == null)
                return NotFound(new ApiResponse<ProductDto>()
                {
                    Success = false,
                    Message = "Product not found , please check the id",
                    Data = null
                });

            //delete old images from folder
            if (getproduct.Photos.Count > 0)
                foreach (var photo in getproduct.Photos)
                    imageSetting.DeleteImage(photo.ImageName);
   
            //update product
            var product = mapper.Map(updateProdcutDto, getproduct);
            //save new images
            var images = ImageSetting.saveImage(updateProdcutDto.Photos, updateProdcutDto.Name);
            //send to  service to update
            var updateproduct = await productServices.UpdateProductAsync(product, images);
            //map to dto
            var productDto = mapper.Map<ProductDto>(updateproduct);
            //return response
            var response = new ApiResponse<ProductDto>()
            {
                Success = true,
                Message = "Product updated successfully",
                Data = productDto
            };
            return Ok(response);

        }

        [HttpGet("home")]
        public async Task<ActionResult<HomeProductsDto>> GetHomePageData()
        {
            var latestSpec = new ProductLatestSpecification();
            var featuredSpec = new ProductFeaturedSpecification();
            var offersSpec = new ProductWithOffersSpecification();

            var latest = await productServices.ListAsync(latestSpec);
            var featured = await productServices.ListAsync(featuredSpec);
            var offers = await productServices.ListAsync(offersSpec);

            return Ok(new HomeProductsDto
            {
                LatestProducts = mapper.Map<IReadOnlyList<HomeProductToReturnDto>>(latest),
                FeaturedProducts = mapper.Map<IReadOnlyList<HomeProductToReturnDto>>(featured),
                OfferProducts = mapper.Map<IReadOnlyList<HomeProductToReturnDto>>(offers)
            });
        }
    }
}