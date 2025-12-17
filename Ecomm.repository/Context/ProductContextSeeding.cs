using Ecomm.core.Entities;
using System.Text.Json;

namespace Ecomm.repository.Context
{
    public class ProductContextSeeding
    {
        public static async Task SeedProducts(DBContext context)
        {
            //if (context.products.Any()) return;



            var productData = File.ReadAllText(
                Path.Combine(Directory.GetCurrentDirectory(), "DataSeed", "products.json")
            );
            var products = JsonSerializer.Deserialize<List<Product>>(productData);

            if (products == null || products.Count == 0) return;

            foreach (var product in products)
            {
                product.Name ??= "Unnamed product";
                product.Description ??= "No description available";
                product.KeySpecs ??= "N/A";

                var photoLinks = product.Photos.Select(p => p.ImageName ?? p.ToString()).ToList();

                product.Photos = photoLinks
                    .Select(link => new Photo
                    {
                        ImageName = link!
                    })
                    .ToList();

                await context.products.AddAsync(product);
            }

            await context.SaveChangesAsync();
        }
    }
}
