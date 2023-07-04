using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using System;

namespace RMS.Domain
{
    public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserRole> UserRole { get; set; }
		public DbSet<Request> Requests { get; set; }
		public DbSet<Lifecycle> Lifecycles { get; set; }
		public DbSet<Category> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Role>().HasData(new Role
			{
				Id = 1,
				Name = "admin",
			});
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = 2,
                Name = "manager",
            });
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = 3,
                Name = "mounter",
            });

            modelBuilder.Entity<User>().HasData(new User
			{
				Id = 1,
				FirstName = "Max",
				Surname = "Akchurin",
				Login = "ADMIN",
				Password = "password",
				Comment = "Comment",
                ImgPath = "../../img/jpg/preview.jpg"
			});
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 2,
                FirstName = "Anton",
                Surname = "Guryshkin",
                Login = "MANAGER",
                Password = "password",
                Comment = "Comment"
            });
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 3,
                FirstName = "Georgii",
                Surname = "Perepelitsa",
                Login = "mounter",
                Password = "password",
                Comment = "Comment"
            });

            modelBuilder.Entity<UserRole>().HasData(new UserRole
			{
				UserRoleId = 1,
				RoleId = 1,
				UserId = 1
			});

            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                UserRoleId = 2,
                RoleId = 2,
                UserId = 2
            });

            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                UserRoleId = 3,
                RoleId = 3,
                UserId = 3
            });

            modelBuilder.Entity<Request>().HasData(new Request
            {
				Id = 1,
				Address = "some address",
				CategoryId = 1,
				Comment = "comment",
				Description = "description",
				LifecycleId = 1,
				Name = "request 1",
				Priority = 1,
				Status = 1,
                CreatedName = "Max Akchurin"
            });
            modelBuilder.Entity<Request>().HasData(new Request
            {
                Id = 2,
                Address = "some address",
                CategoryId = 1,
                Comment = "comment",
                Description = "description",
                LifecycleId = 2,
                Name = "request 2",
                Priority = 2,
                Status = 1,
                CreatedName = "Max Akchurin"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
				Id = 1,
				Name = "No internet",
			});
            modelBuilder.Entity<Lifecycle>().HasData(new Lifecycle
            {
				Id = 1,
				Planning = DateTime.UtcNow
            });
            modelBuilder.Entity<Lifecycle>().HasData(new Lifecycle
            {
                Id = 2,
                Planning = DateTime.UtcNow
            });
        }

	}
}
