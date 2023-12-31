﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using System;
using System.Diagnostics.Metrics;

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
		public DbSet<Brigade> Brigades { get; set; }
		public DbSet<BrigadeMounter> BrigadeMounters { get; set; }


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
				Surname = "Admin",
				Login = "ADMIN",
				Password = SecurePasswordHasher.Hash("aspoqw12"),
				Comment = "Comment",
				ImgPath = "../../img/jpg/preview.jpg",
				ApiKey = "xd"
			});
			modelBuilder.Entity<User>().HasData(new User
			{
				Id = 2,
				FirstName = "Anton",
				Surname = "Manager",
				Login = "MANAGER",
				Password = SecurePasswordHasher.Hash("password"),
				Comment = "Comment"
			});
			modelBuilder.Entity<User>().HasData(new User
			{
				Id = 3,
				FirstName = "Georgii",
				Surname = "Mounter",
				Login = "mounter",
				Password = SecurePasswordHasher.Hash("password"),
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
