using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface IRequestRepository
    {
        IQueryable<Request> GetRequests();
        Request? GetRequestById(uint id);
        IQueryable<Request>? GetRequestByStatus(int status);
        void SaveRequest(Request entity);
        void DeleteRequest(uint id);
    }
}
