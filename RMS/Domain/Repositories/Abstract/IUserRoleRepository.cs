using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface IUserRoleRepository
    {
        IQueryable<UserRole> GetUserRole();
        UserRole? GetUserRoleById(uint? id);
        Task<UserRole?> GetUserRoleByIdAsync(uint? id);
        void SaveUserRole(UserRole entity);
		Task<bool> SaveUserRoleAsync(UserRole entity);
		void DeleteUserRole(uint id);
		Task<bool> DeleteUserRoleAsync(uint id);
	}
}
