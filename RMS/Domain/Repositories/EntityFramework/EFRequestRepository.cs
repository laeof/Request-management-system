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
        public void DeleteRequest(uint id)
        {
            context.Requests.Remove(new Request { Id = id });
            context.SaveChanges();
        }
    }
}
