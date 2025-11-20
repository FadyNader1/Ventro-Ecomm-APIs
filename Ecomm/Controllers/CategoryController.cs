using AutoMapper;
using Ecomm.core.Entities;
using Ecomm.DTOs.CategoryDTOs;
using Ecomm.Errors;
using Ecomm.Responses;
using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecomm.Controllers
{
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly IMapper mapper;
        private readonly ICategoryServices categoryServices;

        public CategoryController(IMapper mapper,ICategoryServices categoryServices)
        {
            this.mapper = mapper;
            this.categoryServices = categoryServices;
        }

        [HttpGet("get-all-categories")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetAllCategories()
        {
            var result = await categoryServices.GetAllCategoriesAsync();
            var response = new ApiResponse<IReadOnlyList<Category>>()
            {
                Success = true,
                Message = "Get all categories successfully",
                Data = result
            };
            return Ok(response);
        }

        [HttpGet("get-category/{id}")]
        public async Task<ActionResult<Category>> GetCategoryById( int id)
        {
            var result = await categoryServices.GetCategoryByIdAsync(id);
            var response = new ApiResponse<Category>()
            {
                Success = true,
                Message = "Get category successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpPost("add-category")]
        public async Task<ActionResult<Category>> AddCategory(AddCategoryDto addCategoryDto)
        {
            var category = mapper.Map<AddCategoryDto, Category>(addCategoryDto);
            if(category == null)
                return BadRequest(new ApiHandleError(400,"Invalid data"));
            var addcategory=await categoryServices.AddCategoryAsync(category);
            var response = new ApiResponse<Category>()
            {
                Success = true,
                Message = "Add category successfully",
                Data = addcategory
            };
            return Ok(response);
        }

        [HttpPut("update-category")]
        public async Task<ActionResult<Category>> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            var category=mapper.Map<UpdateCategoryDto, Category>(updateCategoryDto);
            if (category == null)
                return BadRequest(new ApiHandleError(400, "Invalid data"));

            var updatecategory = await categoryServices.UpdateCategoryAsync(category);
            var response = new ApiResponse<Category>()
            {
                Success = true,
                Message = "Update category successfully",
                Data = updatecategory
            };
            return Ok(response);
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            var result = await categoryServices.DeleteAsync(id);
            var response = new ApiResponse<Category>()
            {
                Success = true,
                Message = "Delete category successfully",
            };
            return Ok(result);
        }


    }
}
