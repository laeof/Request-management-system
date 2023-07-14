using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Repositories.Abstract;
using System.Runtime.CompilerServices;

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
            return context.Categories.Where(x => x.IsDeleted != true);
        }
        public Category? GetCategoryById(uint? id)
        {
            return context.Categories.FirstOrDefault(x => x.Id == id);
        }
        public async Task<Category?> GetCategoryByIdAsync(uint? id) 
        {
			return await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
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
        public async Task<bool> SaveCategoryAsync(Category entity)
        {
			if (entity.Id == default)
			{
				context.Entry(entity).State = EntityState.Added;
			}
			else
				context.Entry(entity).State = EntityState.Modified;

			var saveTask = context.SaveChangesAsync();

			await saveTask;

			return saveTask.IsCompletedSuccessfully;
		}
        public void DeleteCategory(uint id)
        {
            context.Categories.Remove(new Category { Id = id });
            context.SaveChanges();
        }
        public async Task<bool> DeleteCategoryAsync(uint id)
        {
			context.Categories.Remove(new Category { Id = id });

			var saveTask = context.SaveChangesAsync();

			await saveTask;

			return saveTask.IsCompletedSuccessfully;
		}
        public async Task<bool> SoftDeleteCategoryAsync(uint id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(r => r.Id == id);
            if (category != null)
            {
                category.IsDeleted = true;

                await context.SaveChangesAsync();

                return true;
            }
            return false;
        }
    }
}
