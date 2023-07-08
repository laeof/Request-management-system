using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Repositories.Abstract;

namespace RMS.Domain.Repositories.EntityFramework
{
    public class EFUserRepository : IUserRepository
	{
        private readonly AppDbContext context;
        public EFUserRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IQueryable<User> GetUsers()
        {
            return context.Users;
        }
        public User? GetUserById(uint? id)
        {
            return context.Users.FirstOrDefault(x => x.Id == id);
        }
        public async Task<User?> GetUserByIdAsync(uint? id)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        public void SaveUser(User entity)
        {
			if (entity.Id == default)
			{
				context.Entry(entity).State = EntityState.Added;
			}
			else
			{
                if(entity.Password == null)
                {
					entity.Password = context.Users.AsNoTracking().FirstOrDefault(u => u.Id == entity.Id).Password;
				}
				context.Entry(entity).State = EntityState.Modified;
			}

			context.SaveChanges();
		}
        public async Task<bool> SaveUserAsync(User entity)
        {
            if (entity.Id == default)
            {
                context.Entry(entity).State = EntityState.Added;
            }
            else
            {
                if (entity.Password == null)
                {
                    var pw = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == entity.Id);
                    entity.Password = pw.Password;
                }
                context.Entry(entity).State = EntityState.Modified;
            }
            var saveTask = context.SaveChangesAsync();

            await saveTask;

            return saveTask.IsCompletedSuccessfully;
        }
        public void DeleteUser(User user)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }
        public async Task<bool> DeleteUserAsync(User user)
        {
            context.Users.Remove(user);

            var saveTask = context.SaveChangesAsync();

            await saveTask;

            return saveTask.IsCompletedSuccessfully;
        }
    }
}
