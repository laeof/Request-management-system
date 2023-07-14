using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetCategories();
		Category? GetCategoryById(uint? id);
        Task<Category?> GetCategoryByIdAsync(uint? id);
        void SaveCategory(Category entity);
        Task<bool> SaveCategoryAsync(Category entity);
        void DeleteCategory(uint id);
        Task<bool> DeleteCategoryAsync(uint id);
        Task<bool> SoftDeleteCategoryAsync(uint id);
    }
}
