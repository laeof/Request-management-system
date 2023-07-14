using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> GetUsers();
        User? GetUserById(uint? id);
        Task<User?> GetUserByIdAsync(uint? id);
        void SaveUser(User entity);
        Task<bool> SaveUserAsync(User entity);
        void DeleteUser(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<bool> SoftDeleteUserAsync(User user);

	}
}
