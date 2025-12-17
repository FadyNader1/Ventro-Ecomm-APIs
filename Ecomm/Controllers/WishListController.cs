using AutoMapper;
using Ecomm.DTOs.WishListDTOs;
using Ecomm.Responses;
using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : BaseController
    {
        private readonly IWishListService wishListService;
        private readonly IMapper mapper;

        public WishListController(IWishListService wishListService, IMapper mapper)
        {
            this.wishListService = wishListService;
            this.mapper = mapper;
        }
        [Authorize]
        [HttpPost("add-wishlist")]
        public async Task<ActionResult<ApiResponse<WishListDto>>> AddToWishList([FromQuery] int productId)
        {
            var result = await wishListService.AddToWishListAsync( productId);
            var maptoDto = mapper.Map<WishListDto>(result);
            var response = new ApiResponse<WishListDto>()
            {
                Message = "Product added to wish list",
                Data = maptoDto,
                Success = true
            };
            return response;
        }
        [Authorize]
        [HttpGet("get-wishlists-for-current-user")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<WishListDto>>>> GetWishListsForCurrentUser()
        {
            var result = await wishListService.GetWishListByUserIdAsync();
            var maptoDto = mapper.Map<IReadOnlyList<WishListDto>>(result);
            var response = new ApiResponse<IReadOnlyList<WishListDto>>()
            {
                Message = "Wish list retrieved successfully",
                Data = maptoDto,
                Success = true
            };
            return response;
        }
        [Authorize]
        [HttpDelete("remove-from-wishlist")]
        public async Task<ActionResult<ApiResponse<WishListDto>>> RemoveFromWishList([FromQuery] int productId)
        {
            var result = await wishListService.RemoveFromWishListAsync( productId);
            var maptoDto = mapper.Map<WishListDto>(result);
            var response = new ApiResponse<WishListDto>()
            {
                Message = "Product removed from wish list",
                Data = maptoDto,
                Success = true
            };
            return response;
        }
        [Authorize]
        [HttpGet("is-product-in-wishlist")]
        public async Task<ActionResult<ApiResponse<bool>>> IsProductInWishList([FromQuery] int productId)
        {
            var result = await wishListService.IsProductInWishListAsync(productId);
            var response = new ApiResponse<bool>()
            {
                Message = "Check completed successfully",
                Data = result,
                Success = true
            };
            return response;
        }
    }
}
