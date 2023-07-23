using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Repositories.Abstract;
using System.Runtime.CompilerServices;

namespace RMS.Domain.Repositories.EntityFramework
{
    public class EFBrigadeMounterRepository : IBrigadeMounterRepository
    {
        private readonly AppDbContext context;
        public EFBrigadeMounterRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IQueryable<BrigadeMounter> GetBrigadeMounters()
        {
            return context.BrigadeMounters;
        }
        public BrigadeMounter? GetBrigadeMounterById(uint? id)
        {
            return context.BrigadeMounters.FirstOrDefault(x => x.Id == id);
        }
        public async Task<BrigadeMounter?> GetBrigadeMounterByIdAsync(uint? id) 
        {
			return await context.BrigadeMounters.FirstOrDefaultAsync(x => x.Id == id);
		}
        public void SaveBrigadeMounter(BrigadeMounter entity)
        {
            if (entity.Id == default)
            {
                context.Entry(entity).State = EntityState.Added;
            }
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
        public async Task<bool> SaveBrigadeMounterAsync(BrigadeMounter entity)
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
        public void DeleteBrigadeMounter(uint id)
        {
            context.BrigadeMounters.Remove(new BrigadeMounter { Id = id });
            context.SaveChanges();
        }
        public async Task<bool> DeleteBrigadeMounterAsync(uint id)
        {
			context.BrigadeMounters.Remove(new BrigadeMounter { Id = id });

			var saveTask = context.SaveChangesAsync();

			await saveTask;

			return saveTask.IsCompletedSuccessfully;
		}
        public async Task<bool> SoftDeleteBrigadeMounterAsync(uint id)
        {
            var brigade = await context.BrigadeMounters.FirstOrDefaultAsync(r => r.Id == id);
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
