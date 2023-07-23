using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface IBrigadeMounterRepository
    {
        IQueryable<BrigadeMounter> GetBrigadeMounters();
        BrigadeMounter? GetBrigadeMounterById(uint? id);
        Task<BrigadeMounter?> GetBrigadeMounterByIdAsync(uint? id);
        void SaveBrigadeMounter(BrigadeMounter entity);
        Task<bool> SaveBrigadeMounterAsync(BrigadeMounter entity);
        void DeleteBrigadeMounter(uint id);
        Task<bool> DeleteBrigadeMounterAsync(uint id);
        Task<bool> SoftDeleteBrigadeMounterAsync(uint id);
    }
}
