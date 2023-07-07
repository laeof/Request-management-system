using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain.Repositories.Abstract;

namespace RMS.Domain.Repositories.EntityFramework
{
    public class EFRoleRepository : IRoleRepository
	{
        private readonly AppDbContext context;
        public EFRoleRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Role> GetRole()
        {
            return context.Roles;
        }
        public Role? GetRoleById(uint? id)
        {
            return context.Roles.FirstOrDefault(x => x.Id == id);
        }
        public async Task<Role?> GetRoleByIdAsync(uint? id)
        {
            return await context.Roles.FirstOrDefaultAsync(x => x.Id == id);
        }
        public void SaveRole(Role entity)
        {
            if (entity.Id == default)
            {
                context.Entry(entity).State = EntityState.Added;
            }
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
        public async Task<bool> SaveRoleAsync(Role entity)
        {
            try
            {
                if (entity.Id == default)
                {
                    context.Entry(entity).State = EntityState.Added;
                }
                else
                    context.Entry(entity).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
		}
        public void DeleteRole(uint id)
        {
            context.Roles.Remove(new Role { Id = id });
            context.SaveChanges();
        }
        public async Task<bool> DeleteRoleAsync(uint id)
        {
            try
            {
                context.Roles.Remove(new Role { Id = id });
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
		}
    }
}
