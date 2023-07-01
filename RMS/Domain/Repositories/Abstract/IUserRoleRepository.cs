using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface IUserRoleRepository
    {
        IQueryable<UserRole> GetUserRole();
        UserRole? GetUserRoleById(uint? id);
        void SaveUserRole(UserRole entity);
        void DeleteUserRole(uint id);
    }
}
