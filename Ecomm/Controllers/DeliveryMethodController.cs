using AutoMapper;
using Ecomm.core.Entities.OrderEntities;
using Ecomm.DTOs.DeliveryMethodDTOs;
using Ecomm.Errors;
using Ecomm.Responses;
using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryMethodController : BaseController
    {
        private readonly IDeliveryMethodService deliveryMethodService;
        private readonly IMapper mapper;

        public DeliveryMethodController(IDeliveryMethodService deliveryMethodService, IMapper mapper)
        {
            this.deliveryMethodService = deliveryMethodService;
            this.mapper = mapper;
        }

        // GET ALL
        [HttpGet("get-all-delivery-methods")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<DeliveryMethodDto>>>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await deliveryMethodService.GetAllDeliveryMethodsAsync();
            var deliveryMethodsDto = mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryMethods);

            return Ok(new ApiResponse<IReadOnlyList<DeliveryMethodDto>>
            {
                Message = "All Delivery Methods",
                Data = deliveryMethodsDto,
                Success = true
            });
        }

        // ADD
        [HttpPost("add-delivery-method")]
        public async Task<ActionResult<ApiResponse<DeliveryMethodDto>>> AddDeliveryMethod([FromBody] AddDeliveryMethodDto dto)
        {
            var entity = mapper.Map<DeliveryMethod>(dto);
            await deliveryMethodService.AddDeliveryMethodAsync(entity);

            var entityDto = mapper.Map<DeliveryMethodDto>(entity);

            return Ok(new ApiResponse<DeliveryMethodDto>
            {
                Message = "Delivery Method Added Successfully",
                Data = entityDto,
                Success = true
            });
        }

        // GET BY ID
        [HttpGet("get-delivery-method")]
        public async Task<ActionResult<ApiResponse<DeliveryMethodDto>>> GetDeliveryMethod([FromQuery]int id)
        {
            var deliveryMethod = await deliveryMethodService.GetDeliveryMethodByIdAsync(id);
            var dto = mapper.Map<DeliveryMethodDto>(deliveryMethod);

            return Ok(new ApiResponse<DeliveryMethodDto>
            {
                Message = "Delivery Method",
                Data = dto,
                Success = true
            });
        }

        // UPDATE
        [HttpPut("update-delivery-method")]
        public async Task<ActionResult<ApiResponse<DeliveryMethodDto>>> UpdateDeliveryMethod([FromBody] UpdateDeliveryMethodDto dto)
        {
           
            if (dto.Id <= 0)
                return BadRequest(new ApiHandleError(400,"Invalid DeliveryMethod Id"));

            var entity = mapper.Map<DeliveryMethod>(dto);

            await deliveryMethodService.UpdateDeliveryMethodAsync(entity);

            var entityDto = mapper.Map<DeliveryMethodDto>(entity);

            return Ok(new ApiResponse<DeliveryMethodDto>
            {
                Message = "Delivery Method Updated Successfully",
                Data = entityDto,
                Success = true
            });
        }

        // DELETE
        [HttpDelete("delete-delivery-method")]
        public async Task<ActionResult<ApiResponse<DeliveryMethodDto>>> DeleteDeliveryMethod([FromQuery]int id)
        {
            var deliveryMethod = await deliveryMethodService.GetDeliveryMethodByIdAsync(id);

            await deliveryMethodService.DeleteDeliveryMethodAsync(deliveryMethod);

            var dto = mapper.Map<DeliveryMethodDto>(deliveryMethod);

            return Ok(new ApiResponse<DeliveryMethodDto>
            {
                Message = "Delivery Method Deleted Successfully",
                Data = dto,
                Success = true
            });
        }
    }
}
