using Ecomm.core.DTOs.Basket;
using Ecomm.core.Entities.BasketEntities;
using Ecomm.core.Entities.OrderEntities;
using Ecomm.core.Exceptions;
using Ecomm.service.InterfaceServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomm.service.ImplementServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomerBasket _customerBasket;
        private readonly IDeliveryMethodService _deliveryMethodService;
        private readonly IProductServices _productServices;

        public PaymentService(
            IConfiguration configuration,
            ICustomerBasket customerBasket,
            IDeliveryMethodService deliveryMethodService,
            IProductServices productServices)
        {
            _configuration = configuration;
            _customerBasket = customerBasket;
            _deliveryMethodService = deliveryMethodService;
            _productServices = productServices;
        }

        public async Task<CustomerBasket> CreatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secret_key"];

            // 1. Fetch basket
            var basket = await _customerBasket.GetCustomerBasket(basketId);
            if (basket is null) throw new BadRequestException("Basket not found");

            if (string.IsNullOrEmpty(basket.BasketId))
                basket.BasketId = basketId;

            if (basket.Items is null || !basket.Items.Any())
                throw new BadRequestException("Basket has no items");

            // 2. Get shipping price
            decimal shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _deliveryMethodService.GetDeliveryMethodByIdAsync(basket.DeliveryMethodId.Value);
                if (deliveryMethod is not null)
                    shippingPrice = deliveryMethod.Price;
            }

            // 3. Update product prices from DB (To prevent price tampering)
            foreach (var item in basket.Items)
            {
                var product = await _productServices.GetProductByIdAsync(item.ProductId);
                if (product is null)
                    throw new BadRequestException($"Product with id {item.ProductId} not found.");

                item.ProductName = product.Name;
                item.NewPrice = product.NewPrice;
            }

            // 4. Calculate total amount in cents
            long amount = (long)((basket.Items.Sum(x => x.NewPrice * x.Quantity) + shippingPrice) * 100);

            var paymentService = new PaymentIntentService();
            PaymentIntent intent;

            // --- المنطق المحدث هنا ---

            bool shouldCreateNewIntent = string.IsNullOrEmpty(basket.PaymentIntentId);

            // إذا كان هناك Intent ID، نتحقق من حالته في Stripe أولاً
            if (!shouldCreateNewIntent)
            {
                try
                {
                    var existingIntent = await paymentService.GetAsync(basket.PaymentIntentId);

                    // إذا كانت العملية ناجحة بالفعل، لا يمكن تحديثها، فننشئ واحدة جديدة
                    if (existingIntent.Status == "succeeded")
                    {
                        shouldCreateNewIntent = true;
                    }
                }
                catch (StripeException)
                {
                    // في حالة وجود ID خطأ أو غير موجود في Stripe
                    shouldCreateNewIntent = true;
                }
            }

            // 5. Create or Update logic
            if (shouldCreateNewIntent)
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await paymentService.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                // هنا نحن متأكدون أن الـ Intent حالته تسمح بالتحديث
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = amount
                };

                intent = await paymentService.UpdateAsync(basket.PaymentIntentId, options);
                basket.ClientSecret = intent.ClientSecret;
            }

            // 6. Save updated basket
            var updatedBasket = await _customerBasket.UpdateBasket(basket);
            return updatedBasket;
        }
    }
}
