using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.DTOs.Identity
{
    public class ApiResponseAuth
    {
        public string Message { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public String? Token { get; set; }
        public string? RefreshToken { get; set; }

    }
}
