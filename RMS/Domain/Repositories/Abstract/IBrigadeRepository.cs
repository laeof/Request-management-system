using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface IBrigadeRepository
    {
        IQueryable<Brigade> GetBrigades();
		Brigade? GetBrigadeById(uint? id);
        Task<Brigade?> GetBrigadeByIdAsync(uint? id);
        void SaveBrigade(Brigade entity);
        Task<bool> SaveBrigadeAsync(Brigade entity);
        void DeleteBrigade(uint id);
        Task<bool> DeleteBrigadeAsync(uint id);
        Task<bool> SoftDeleteBrigadeAsync(uint id);
    }
}
