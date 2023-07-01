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
        public void DeleteLifecycle(uint id)
        {
            context.Categories.Remove(new Category { Id = id });
            context.SaveChanges();
        }
    }
}
