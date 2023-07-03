using RMS.Domain.Entities;

namespace RMS.Domain.Repositories.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> GetUsers();
        User? GetUserById(uint? id);
        void SaveUser(User entity);
        void DeleteUser(User user);
    }
}
