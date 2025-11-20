using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.DTOs.CategoryDTOs
{
    public record AddCategoryDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = " Name must be at least 3 characters long")]
        [MaxLength(100, ErrorMessage = " Name must be at most 100 characters long")]
        public string Name { get; init; } = default!;
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description must be at most 500 characters long")]
        [MinLength(3, ErrorMessage = "Description must be at least 3 characters long")]
        public string Description { get; init; } = default!;
    }
}