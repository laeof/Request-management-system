using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface ILifecycleRepository
    {
        IQueryable<Lifecycle> GetLifecycles();
		Lifecycle GetLifecycleById(uint id);
        void SaveLifecycle(Lifecycle entity);
        void DeleteLifecycle(uint id);
    }
}
