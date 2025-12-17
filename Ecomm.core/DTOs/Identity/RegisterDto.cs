using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.DTOs.Identity
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(50, ErrorMessage = "First Name must be less than 50 characters")]
        [MinLength(3,ErrorMessage ="First Name must be at least 3 characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(50,ErrorMessage ="Last Name must be less than 50 characters")]
        [MinLength(3, ErrorMessage = "Last Name must be at least 3 characters")]
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^[A-Z][a-z]+[0-9]+$",ErrorMessage = "Password must start with a capital letter, followed by letters, then numbers.")]
        public string Password { get; set; }

    }
}
