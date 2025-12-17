using Ecomm.core.Entities.IdentityModle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Entities
{
    public class RefreshToken:BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRevoked { get; set; }
    }
}
