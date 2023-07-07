using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface IRoleRepository
    {
        IQueryable<Role> GetRole();
        Role? GetRoleById(uint? id);
        Task<Role?> GetRoleByIdAsync(uint? id);
        void SaveRole(Role entity);
        Task<bool> SaveRoleAsync(Role entity);
        void DeleteRole(uint id);
        Task<bool> DeleteRoleAsync(uint id);
    }
}
