using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Repositories.Abstract;

namespace RMS.Domain.Repositories.EntityFramework
{
    public class EFLifecycleRepository : ILifecycleRepository
	{
        private readonly AppDbContext context;
        public EFLifecycleRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Lifecycle> GetLifecycles()
        {
            return context.Lifecycles;
        }
        public Lifecycle GetLifecycleById(uint id)
        {
            if (context.Lifecycles.FirstOrDefault(x => x.Id == id) == null)
                throw new Exception();

            return context.Lifecycles.FirstOrDefault(x => x.Id == id);
        }
        public async Task<Lifecycle> GetLifecycleByIdAsync(uint id)
        {
            var getLifecycles = context.Lifecycles.FirstOrDefaultAsync(x => x.Id == id);

            await getLifecycles;

            if(getLifecycles == null)
                throw new Exception();

            if (getLifecycles.IsCompletedSuccessfully)
            {
                return getLifecycles.Result;
            }
            else
                throw new Exception();
		}
        public void SaveLifecycle(Lifecycle entity)
        {
            if (entity.Id == default)
            {
                context.Entry(entity).State = EntityState.Added;
            }
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
        public async Task<bool> SaveLifecycleAsync(Lifecycle entity)
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
        public void DeleteLifecycle(uint id)
        {
            context.Categories.Remove(new Category { Id = id });
            context.SaveChanges();
        }
        public async Task<bool> DeleteLifecycleAsync(uint id)
        {
			context.Categories.Remove(new Category { Id = id });

			var saveTask = context.SaveChangesAsync();

			await saveTask;

			return saveTask.IsCompletedSuccessfully;
		}
    }
}
