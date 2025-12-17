using Ecomm.core.DTOs.Basket;
using Ecomm.core.Entities;
using Ecomm.core.Entities.BasketEntities;
using Ecomm.core.Entities.OrderEntities;
using Ecomm.core.Exceptions;
using Ecomm.core.Interfaces;
using Ecomm.service.InterfaceServices;
using Org.BouncyCastle.Bcpg;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecomm.service.ImplementServices
{
    public class CustomerBasketService : ICustomerBasket
    {
        private readonly IDatabase database;
        private readonly IProductRepo productRepo;

        public CustomerBasketService(IConnectionMultiplexer redis,IProductRepo productRepo)
        {
            this.database = redis.GetDatabase();
            this.productRepo = productRepo;
        }
        public async Task<CustomerBasket> AddItemToBasket(AddToBasketDto addToBasketDto)
        {
            if (string.IsNullOrEmpty(addToBasketDto.BasketId))
                throw new BadRequestException("BasketId is required");

            // 1) Get existing basket OR create new
            CustomerBasket basket;
            try
            {
                basket = await GetCustomerBasket(addToBasketDto.BasketId);
            }
            catch (NotFoundException)
            {
                basket = new CustomerBasket(addToBasketDto.BasketId);
            }

            // ============================
            // (A) UPDATE BASKET FIELDS
            // ============================
            if (addToBasketDto.DeliveryMethodId.HasValue)
                basket.DeliveryMethodId = addToBasketDto.DeliveryMethodId.Value;

            if (addToBasketDto.shippingAddress != null)
                basket.shippingAddress = addToBasketDto.shippingAddress;

            if (addToBasketDto.ShipPrice.HasValue)
                basket.ShippingPrice = addToBasketDto.ShipPrice.Value;

            if (!string.IsNullOrEmpty(addToBasketDto.EmailBuyer))
                basket.EmailBuyer = addToBasketDto.EmailBuyer;

            // ============================
            // (B) ADD / UPDATE PRODUCT
            // ============================

            // لو مفيش ProductId → نعمل Update فقط بدون Add
            if (addToBasketDto.ProductId.HasValue && addToBasketDto.Quantity.HasValue)
            {
                var product = await productRepo.GetProductByIdAsync(addToBasketDto.ProductId.Value);
                if (product == null)
                    throw new NotFoundException($"Product with id {addToBasketDto.ProductId} not found");

                var existingItem = basket.Items.FirstOrDefault(x => x.ProductId == addToBasketDto.ProductId);

                var photos = product.Photos.Select(x => x.ImageName).ToList();

                if (existingItem == null)
                {
                    basket.Items.Add(new ItemBasket(
                        addToBasketDto.ProductId.Value,
                        product.Name,
                        product.Description,
                        product.OldPrice,
                        product.NewPrice,
                        photos,
                        addToBasketDto.Quantity.Value
                    ));
                }
                else
                {
                    existingItem.Quantity += addToBasketDto.Quantity.Value;
                }
            }

            // ============================
            // (C) SAVE TO REDIS
            // ============================
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            await database.StringSetAsync(addToBasketDto.BasketId, JsonSerializer.Serialize(basket, options));
            await database.KeyExpireAsync(addToBasketDto.BasketId, TimeSpan.FromDays(30));

            return basket;
        }

        public async Task<bool> ClearBasket(string basketId)
        {
            return await database.KeyDeleteAsync(basketId);
        }

        public string CreateBasket()
        {
            var basket = new CustomerBasket(Guid.NewGuid().ToString());
            return basket.BasketId;

        }

        public async Task<CustomerBasket> GetCustomerBasket(string basketId)
        {
            if (basketId == null)
                throw new BadRequestException("BasketId is not valid");

            var basket = await database.StringGetAsync(basketId);

            if (string.IsNullOrEmpty(basket))
            {
                // فقط لو السلة فعليًا مش موجودة
                var newBasket = new CustomerBasket(basketId);
                // لا تكتبها على Redis هنا! فقط ارجعها
                return newBasket;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var customerBasket = JsonSerializer.Deserialize<CustomerBasket>(basket!, options);
            customerBasket.BasketId= basketId ; 

            return customerBasket!;
        }

        public async Task<bool> RemoveItemFromBasket(string BasketId, int productId)
        {
            if (!string.IsNullOrEmpty(BasketId))
            {
                var basket =await GetCustomerBasket(BasketId);
                var item = basket.Items.FirstOrDefault(x => x.ProductId == productId);
                if (item == null)
                    throw new NotFoundException($"Product with id {productId} not found in basket");
                basket.Items.Remove(item);
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                return await database.StringSetAsync(BasketId, JsonSerializer.Serialize(basket,options));
               
            }
            throw new BadRequestException("BasketId is not valid");
        }

        public async Task<CustomerBasket> UpdateBasket(CustomerBasket basket)
        {
            if (basket == null || string.IsNullOrEmpty(basket.BasketId))
                throw new BadRequestException("Basket is invalid");

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                var serializedBasket = JsonSerializer.Serialize(basket, options);

                bool created = await database.StringSetAsync(
                    basket.BasketId,
                    serializedBasket,
                    TimeSpan.FromDays(30)
                );

                Console.WriteLine($"Redis StringSetAsync result for BasketId={basket.BasketId}: {created}");

                if (!created)
                {
                    created = await database.StringSetAsync(
                        basket.BasketId,
                        serializedBasket,
                        TimeSpan.FromDays(30)
                    );
                    if (!created)
                    {
                        throw new Exception("Failed to update basket in database after retry");
                    }
                }

                return basket;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateBasket Exception: {ex.Message}");
                throw;
            }
        }




    }
}
