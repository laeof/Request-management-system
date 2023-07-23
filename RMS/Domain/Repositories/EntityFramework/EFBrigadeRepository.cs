using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Repositories.Abstract;
using System.Runtime.CompilerServices;

namespace RMS.Domain.Repositories.EntityFramework
{
    public class EFBrigadeRepository : IBrigadeRepository
	{
        private readonly AppDbContext context;
        public EFBrigadeRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Brigade> GetBrigades()
        {
            return context.Brigades.Include(b => b.BrigadeMounters)
                .ThenInclude(bm => bm.Mounter).Where(x => x.IsDeleted != true);
        }
        public Brigade? GetBrigadeById(uint? id)
        {
            return context.Brigades.FirstOrDefault(x => x.Id == id);
        }
        public async Task<Brigade?> GetBrigadeByIdAsync(uint? id) 
        {
			return await context.Brigades.FirstOrDefaultAsync(x => x.Id == id);
		}
        public void SaveBrigade(Brigade entity)
        {
            if (entity.Id == default)
            {
                context.Entry(entity).State = EntityState.Added;
            }
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
        public async Task<bool> SaveBrigadeAsync(Brigade entity)
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
        public void DeleteBrigade(uint id)
        {
            context.Brigades.Remove(new Brigade { Id = id });
            context.SaveChanges();
        }
        public async Task<bool> DeleteBrigadeAsync(uint id)
        {
			context.Brigades.Remove(new Brigade { Id = id });

			var saveTask = context.SaveChangesAsync();

			await saveTask;

			return saveTask.IsCompletedSuccessfully;
		}
        public async Task<bool> SoftDeleteBrigadeAsync(uint id)
        {
            var brigade = await context.Brigades.FirstOrDefaultAsync(r => r.Id == id);
            if (brigade != null)
            {
				brigade.IsDeleted = true;

                await context.SaveChangesAsync();

                return true;
            }
            return false;
        }
    }
}
