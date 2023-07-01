using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface IRoleRepository
    {
        IQueryable<Role> GetRole();
        Role? GetRoleById(uint? id);
        void SaveRole(Role entity);
        void DeleteRole(uint id);
    }
}
