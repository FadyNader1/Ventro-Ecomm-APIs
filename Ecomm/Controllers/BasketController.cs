using Ecomm.core.DTOs.Basket;
using Ecomm.core.Entities.BasketEntities;
using Ecomm.core.Entities.OrderEntities;
using Ecomm.Responses;
using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : BaseController
    {
        private readonly ICustomerBasket customerBasket;

        public BasketController(ICustomerBasket customerBasket)
        {
            this.customerBasket = customerBasket;
        }

        [HttpGet("CreateBasketId")]
        public  ActionResult<string> CreateBasketId()
        {
            var basketId = customerBasket.CreateBasket();
            return Ok(new { basketId = basketId });
        }
        [HttpPost("AddToBasket")]
        public async Task<ActionResult<ApiResponse<CustomerBasket>>> AddToBasket([FromBody] AddToBasketDto addToBasketDto)
        {
            var baseket= await customerBasket.AddItemToBasket(addToBasketDto);
            var response = new ApiResponse<CustomerBasket>()
            {
                Data = baseket,
                Message = "Item added to basket successfully",
                Success = true
            };
            return Ok(response);
        }
        [HttpDelete("RemoveItemFromBasket")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveFromBasket([FromQuery]string basketId,[FromQuery] int productId)
        {
            var result =await customerBasket.RemoveItemFromBasket(basketId, productId);
            var response = new ApiResponse<bool>()
            {
                Message = result ? "Item removed from basket successfully" : "Failed to remove item from basket",
                Success = result,
                Data = result
            };
            return Ok(response);
        }
        [HttpGet("GetBasket")]
        public async Task<ActionResult<ApiResponse<CustomerBasket>>> GetBasket([FromQuery]string basketId)
        {
            var basket = await customerBasket.GetCustomerBasket(basketId);
            var response = new ApiResponse<CustomerBasket>()
            {
                Data = basket,
                Message = "Basket retrieved successfully",
                Success = true
            };
            return Ok(response);
        }
        [HttpDelete("ClearBasket")]
        public async Task<ActionResult<ApiResponse<bool>>> ClearBasket(string basketId)
        {
            var result = await customerBasket.ClearBasket(basketId);
            var response = new ApiResponse<bool>()
            {
                Message = result ? "Basket cleared successfully" : "Failed to clear basket",
                Success = result,
                Data = result
            };
            return Ok(response);
        }   
    }
}
