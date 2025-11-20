using Ecomm.core.Entities;
using Ecomm.core.Interfaces;
using Ecomm.repository.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.repository.Repository
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly DBContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CategoryRepo> logger;

        public CategoryRepo(DBContext context, IUnitOfWork unitOfWork, ILogger<CategoryRepo> logger)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task<bool> AddCategoryAsync(Category category)
        {
            try
            {
                await unitOfWork.Repository<Category>().AddAsync(category);
                await unitOfWork.CompleteAsync();
                return true;
            } catch (Exception ex)
            {
                logger.LogError(ex, "Error adding category");
                return false;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await unitOfWork.Repository<Category>().GetByIdAsync(id);
                if (category == null)
                {
                    logger.LogWarning("Category with id {Id} not found", id);
                    return false;
                }
                unitOfWork.Repository<Category>().Delete(category);
                await unitOfWork.CompleteAsync();
                return true;
            } catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting category");
                return false;
            }
        }

        public async Task<IReadOnlyList<Category>> GetAllCategoriesAsync()
        {
            var categories = await unitOfWork.Repository<Category>().GetAllAsync();
            return categories;

        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(id);
            return category;
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            try
            {
                unitOfWork.Repository<Category>().Update(category);
                await unitOfWork.CompleteAsync();
                return true;
            } catch (Exception ex)
            {
                logger.LogError(ex, "Error updating category");
                return false;
            }
        }
    }
}