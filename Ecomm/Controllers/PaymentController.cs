using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost("Create-payment-intent")]
        public async Task<ActionResult> CreatePaymentIntent([FromQuery] string basketId)
        {
            Console.WriteLine($"=>=>=> BasketId: {basketId}");

            var basket = await paymentService.CreatePaymentIntentAsync(basketId);
            return Ok(basket);
        }
    }
}
