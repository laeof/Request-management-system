﻿using Microsoft.EntityFrameworkCore;
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
		public void SaveUser(User entity)
        {
            if (entity.Id == default)
            {
                context.Entry(entity).State = EntityState.Added;
            }
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void DeleteUser(uint id)
        {
            context.Users.Remove(new User { Id = id });
            context.SaveChanges();
        }
    }
}
