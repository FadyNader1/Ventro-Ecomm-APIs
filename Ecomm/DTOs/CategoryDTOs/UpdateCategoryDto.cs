using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.DTOs.CategoryDTOs
{
    public record UpdateCategoryDto
    {
        [Required(ErrorMessage ="Id is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name should be less than 100 characters")]
        [MinLength(3, ErrorMessage = "Name should be more than 3 characters")]
        public string Name { get; set; } = default!;
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description must be at most 500 characters long")]
        [MinLength(3, ErrorMessage = "Description must be at least 3 characters long")]
        public string Description { get; init; } = default!;
    }
}
