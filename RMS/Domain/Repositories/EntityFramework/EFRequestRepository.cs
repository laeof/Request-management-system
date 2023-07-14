using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Repositories.Abstract;

namespace RMS.Domain.Repositories.EntityFramework
{
    public class EFRequestRepository : IRequestRepository
    {
        private readonly AppDbContext context;
        public EFRequestRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Request> GetRequests()
        {
            return context.Requests;
        }
        public Request? GetRequestById(uint id)
        {
            return context.Requests.FirstOrDefault(x => x.Id == id);
        }
        public async Task<Request?> GetRequestByIdAsync(uint id)
        {
            return await context.Requests.FirstOrDefaultAsync(x => x.Id == id);
        }
		public IQueryable<Request>? GetRequestByStatus(int id)
		{
			return context.Requests.Where(x => x.Status == id);
		}
		public void SaveRequest(Request entity)
        {
            if (entity.Id == default)
            {
                context.Entry(entity).State = EntityState.Added;
            }
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
        public async Task<bool> SaveRequestAsync(Request entity)
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
        public void DeleteRequest(uint id)
        {
            context.Requests.Remove(new Request { Id = id });
            context.SaveChanges();
        }
        public async Task<bool> SoftDeleteRequestAsync(uint id) 
        {
            var request = await context.Requests.FirstOrDefaultAsync(r => r.Id == id);
            if (request != null)
            {
                request.IsDeleted = true;

                await context.SaveChangesAsync();

                return true;
            }
            return false;
        }
        public async Task<bool> DeleteRequestAsync(uint id)
        {
			context.Requests.Remove(new Request { Id = id });

			var saveTask = context.SaveChangesAsync();

			await saveTask;

			return saveTask.IsCompletedSuccessfully;
		}
    }
}
