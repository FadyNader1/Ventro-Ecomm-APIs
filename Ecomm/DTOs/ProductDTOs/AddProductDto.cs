using Ecomm.core.Entities;

namespace Ecomm.DTOs.ProductDTOs
{
    public class AddProductDto
    {
        public string Name { get; set; } 
        public string Description { get; set; }
        public string? KeySpecs { get; set; }

        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public  int  CategoryId { get; set; }
        public  IFormFileCollection Photos { get; set; }

        public int InventoryQuantity { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsNewArrival { get; set; } = true;
    }
}
