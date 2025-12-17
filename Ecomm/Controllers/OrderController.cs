using AutoMapper;
using Ecomm.core.DTOs.Order;
using Ecomm.core.Entities.OrderEntities;
using Ecomm.core.Exceptions;
using Ecomm.DTOs.OrderDTOs;
using Ecomm.Errors;
using Ecomm.service.ImplementServices;
using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Ecomm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;
        private readonly IPdfGenerator pdfGenerator;

        public OrderController(IOrderService orderService,IMapper mapper,IPdfGenerator pdfGenerator)
        {
            this.orderService = orderService;
            this.mapper = mapper;
            this.pdfGenerator = pdfGenerator;
        }

        [Authorize]
        [HttpPost("create-order")]
        public async Task<ActionResult<OrderDtoResponse>> CreateOrder(OrderDto OrderDto)
        {
            var emailBuyer = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (emailBuyer == null)
                return Unauthorized(new ApiHandleError(401, "User not found, please login again"));
            var order =await orderService.CreateOrder(OrderDto, emailBuyer);
            var orderDto = mapper.Map<OrderDtoResponse>(order);
            return Ok(orderDto);
        }
        [HttpGet("get-order")]
        [Authorize]
        public async Task<ActionResult<OrderDtoResponse>> GetOrder(int id)
        {
            var emailBuyer = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (emailBuyer == null)
                return Unauthorized(new ApiHandleError(401, "User not found, please login again"));
            var order = await orderService.GetOrderById(id,emailBuyer);
            if(order == null)
                return NotFound(new ApiHandleError(404, "Order not found"));
            var orderDto = mapper.Map<OrderDtoResponse>(order);
            return Ok(orderDto);
        }

        [HttpGet("get-orders-for-user")]
        [Authorize]
        public async Task<ActionResult<List<OrderDtoResponse>>> GetOrdersForUser()
        {
            var emailBuyer = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (emailBuyer == null)
                return Unauthorized(new ApiHandleError(401, "User not found, please login again"));
            var orders = await orderService.GetOrdersForUserAsync(emailBuyer);
            var orderlistDto = mapper.Map<IReadOnlyList<OrderDtoResponse>>(orders);
            return Ok(orderlistDto);
        }

        [HttpGet("{orderId}/invoice-pdf")]
        public async Task<IActionResult> GetInvoicePdf(int orderId)
        {
            var email= HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return Unauthorized(new ApiHandleError(401, "User not found, please login again"));

            var order = await orderService.GetOrderById(orderId,email);

            if (order == null)
                return NotFound();

            var pdfBytes = pdfGenerator.GenerateInvoicePdf(order); // injected

            return File(pdfBytes, "application/pdf", $"Ventro-Invoice-#{orderId}.pdf");
        }
    }
}
