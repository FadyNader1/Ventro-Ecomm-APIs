using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Entities.BasketEntities
{
    public class ItemBasket
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public List<string> Photos { get; set; } = new List<string>();
        public int Quantity { get; set; }

        

        public ItemBasket(int productId, string productName, string description, decimal oldPrice, decimal newPrice, List<string> photos, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            Description = description;
            OldPrice = oldPrice;
            NewPrice = newPrice;
            Photos = photos;
            Quantity = quantity;
        }
    }
}
