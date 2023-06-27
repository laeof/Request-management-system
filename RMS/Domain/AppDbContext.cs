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
			modelBuilder.Entity<UserModel>().HasData(new UserModel
			{
				Id = 1,
				FirstName = "Max",
				Surname = "Akchurin",
				Login = "ADMIN",
				Password = "password",
				Comment = "Comment"
			});
			modelBuilder.Entity<UserRoleModel>().HasData(new UserRoleModel
			{
				UserRoleId = 1,
				RoleId = 1,
				UserId = 1
			});
		}

	}
}
