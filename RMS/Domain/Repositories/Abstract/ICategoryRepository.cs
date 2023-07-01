using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetCategories();
		Category? GetCategoryById(uint? id);
        void SaveCategory(Category entity);
        void DeleteCategory(uint id);
    }
}
