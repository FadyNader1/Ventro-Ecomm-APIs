using Ecomm.core.Entities;
using Ecomm.core.Exceptions;
using Ecomm.core.Interfaces;
using Ecomm.service.InterfaceServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.ImplementServices
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepo categoryRepo;
        private readonly IUnitOfWork unitOfWork;

        public CategoryServices(ICategoryRepo categoryRepo, IUnitOfWork unitOfWork)
        {
            this.categoryRepo = categoryRepo;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Category> AddCategoryAsync(Category category)
        {

            var result = await categoryRepo.AddCategoryAsync(category);
            if (result)
                return category;
            throw new Exception("Category not added");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException("Category not found, please check the id");
            var result = await categoryRepo.DeleteCategoryAsync(id);
            if (result)
                return true;
            throw new Exception("Category not deleted");

        }

        public async Task<IReadOnlyList<Category>> GetAllCategoriesAsync()
        {
            var result = await categoryRepo.GetAllCategoriesAsync();
            return result;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException("Category not found, please check the id");
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            if (category.Id <= 0)
                throw new BadRequestException("Category not found, please check the id");

            var checkCategory = await unitOfWork.Repository<Category>().GetByIdAsync(category.Id);
            if (checkCategory == null)
                throw new NotFoundException("Category not found, please check the id");

            checkCategory.Name = category.Name;
            checkCategory.Description = category.Description;

            var result = await categoryRepo.UpdateCategoryAsync(checkCategory);
            if (result)
                return category;
            throw new Exception("Category not updated");
        }
    }
}