using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface IRequestRepository
    {
        IQueryable<Request> GetRequests();
        Request? GetRequestById(uint id);
        Task<Request?> GetRequestByIdAsync(uint id);
        IQueryable<Request>? GetRequestByStatus(int status);
        void SaveRequest(Request entity);
        Task<bool> SaveRequestAsync(Request entity);
        void DeleteRequest(uint id);
        Task<bool> DeleteRequestAsync(uint id);
        Task<bool> SoftDeleteRequestAsync(uint id);
    }
}
