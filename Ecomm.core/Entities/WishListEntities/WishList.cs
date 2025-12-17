using Ecomm.core.Entities.IdentityModle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Entities.WishListEntities
{
    public class WishList:BaseEntity
    {
        public string AppUserId { get; set; }
        public int ProductId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public AppUser AppUser { get; set; } = default!;
        public Product Product { get; set; }= default!;

    }
}
