using Ecomm.core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecomm.DTOs.ProductDTOs
{
    public record ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? KeySpecs { get; set; }

        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public virtual Category Category { get; set; } 
        public virtual List<string> Photos { get; set; }
        public int inventoryQuantity { get; set; }

        public bool IsFeatured { get; set; }
        public bool IsNewArrival { get; set; }
        public int Rating { get; set; }
    }
}
