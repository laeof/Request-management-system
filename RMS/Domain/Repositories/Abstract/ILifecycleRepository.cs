using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface ILifecycleRepository
    {
        IQueryable<Lifecycle> GetLifecycles();
		Lifecycle GetLifecycleById(uint id);
        Task<Lifecycle> GetLifecycleByIdAsync(uint id);
        void SaveLifecycle(Lifecycle entity);
        Task<bool> SaveLifecycleAsync(Lifecycle entity);
        void DeleteLifecycle(uint id);
        Task<bool> DeleteLifecycleAsync(uint id);
    }
}
