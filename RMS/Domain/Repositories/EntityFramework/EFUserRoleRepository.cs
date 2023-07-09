using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Repositories.Abstract;

namespace RMS.Domain.Repositories.EntityFramework
{
    public class EFUserRoleRepository : IUserRoleRepository
	{
        private readonly AppDbContext context;
        public EFUserRoleRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IQueryable<UserRole> GetUserRole()
        {
            return context.UserRole.AsNoTracking();
        }
        public UserRole? GetUserRoleById(uint? id)
        {
            return context.UserRole.FirstOrDefault(x => x.UserRoleId == id);
        }
        public async Task<UserRole?> GetUserRoleByIdAsync(uint? id)
        {
            return await context.UserRole.FirstOrDefaultAsync(x => x.UserRoleId == id);
        }
        public void SaveUserRole(UserRole entity)
        {
			if (entity.UserRoleId == default)
			{
				context.Entry(entity).State = EntityState.Added;
			}
			else
			{
				context.Entry(entity).State = EntityState.Modified;
			}
			context.SaveChanges();
		}
        public async Task<bool> SaveUserRoleAsync(UserRole entity)
        {
            if (entity.UserRoleId == default)
            {
                context.Entry(entity).State = EntityState.Added;
            }
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }
            var saveTask = context.SaveChangesAsync();

            await saveTask;

            return saveTask.IsCompletedSuccessfully;
		}
        public void DeleteUserRole(uint id)
        {
            context.UserRole.Remove(new UserRole { UserRoleId = id });
            context.SaveChanges();
        }
        public async Task<bool> DeleteUserRoleAsync(uint id)
        {
            context.UserRole.Remove(new UserRole { UserRoleId = id });

			var saveTask = context.SaveChangesAsync();

			await saveTask;

			return saveTask.IsCompletedSuccessfully;

		}
    }
}
