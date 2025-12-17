using Ecomm.DTOs.ProductDTOs;

namespace Ecomm.DTOs.WishListDTOs
{
    public class WishListDto
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } = default!;
        public ProductDto product { get; set; }= default!;
        public DateTime CreatedDate { get; set; }
    }
}
