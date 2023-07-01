using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Repositories.Abstract;

namespace RMS.Domain.Repositories.EntityFramework
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext context;
        public EFCategoryRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Category> GetCategories()
        {
            return context.Categories;
        }
        public Category? GetCategoryById(uint? id)
        {
            return context.Categories.FirstOrDefault(x => x.Id == id);
        }
        public void SaveCategory(Category entity)
        {
            if (entity.Id == default)
            {
                context.Entry(entity).State = EntityState.Added;
            }
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void DeleteCategory(uint id)
        {
            context.Categories.Remove(new Category { Id = id });
            context.SaveChanges();
        }
    }
}
