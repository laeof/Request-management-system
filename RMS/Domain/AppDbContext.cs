using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Models;
using System;

namespace RMS.Domain
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<RoleModel> Roles { get; set; }
		public DbSet<UserModel> Users { get; set; }
		public DbSet<UserRoleModel> UserRole { get; set; }
		public DbSet<RequestModel> Requests { get; set; }
		public DbSet<LifecycleModel> Lifecycles { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<RoleModel>().HasData(new RoleModel
			{
				Id = 1,
				Name = "admin",
			});
            modelBuilder.Entity<RoleModel>().HasData(new RoleModel
            {
                Id = 2,
                Name = "manager",
            });
            modelBuilder.Entity<RoleModel>().HasData(new RoleModel
            {
                Id = 3,
                Name = "mounter",
            });

            modelBuilder.Entity<UserModel>().HasData(new UserModel
			{
				Id = 1,
				FirstName = "Max",
				Surname = "Akchurin",
				Login = "ADMIN",
				Password = "password",
				Comment = "Comment"
			});
            modelBuilder.Entity<UserModel>().HasData(new UserModel
            {
                Id = 2,
                FirstName = "Anton",
                Surname = "Guryshkin",
                Login = "MANAGER",
                Password = "password",
                Comment = "Comment"
            });
            modelBuilder.Entity<UserModel>().HasData(new UserModel
            {
                Id = 3,
                FirstName = "Georgii",
                Surname = "Perepelitsa",
                Login = "mounter",
                Password = "password",
                Comment = "Comment"
            });

            modelBuilder.Entity<UserRoleModel>().HasData(new UserRoleModel
			{
				UserRoleId = 1,
				RoleId = 1,
				UserId = 1
			});

            modelBuilder.Entity<UserRoleModel>().HasData(new UserRoleModel
            {
                UserRoleId = 2,
                RoleId = 2,
                UserId = 2
            });

            modelBuilder.Entity<UserRoleModel>().HasData(new UserRoleModel
            {
                UserRoleId = 3,
                RoleId = 3,
                UserId = 3
            });

            modelBuilder.Entity<RequestModel>().HasData(new RequestModel
            {
				Id = 1,
				Address = "some address",
				CategoryId = 1,
				Comment = "comment",
				Description = "description",
				ExecutorId = 1,
				LifecycleId = 1,
				Name = "request 1",
				Priority = 1,
				Status = 1
            });
			modelBuilder.Entity<CategoryModel>().HasData(new CategoryModel
            {
				Id = 1,
				Name = "No internet",
			});
            modelBuilder.Entity<LifecycleModel>().HasData(new LifecycleModel
            {
				Id = 1,
				Opened = DateTime.UtcNow
            });
        }

	}
}
