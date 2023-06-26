using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;

namespace RMS.Domain
{
	public class AppDbContext : IdentityDbContext<IdentityUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<TextField> TextFields { get; set; }
		public DbSet<ServiceItem> ServiceItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
			{
				Id = "3c2ffd08-736b-47bd-951b-c3b1cac88240",
				Name = "admin",
				NormalizedName = "ADMIN"
			});
			modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
			{
				Id = "20f97c37-e683-4573-aac0-ac2ae70edd49",
				UserName = "admin",
				NormalizedUserName = "ADMIN",
				Email = "a@gmail.com",
				NormalizedEmail = "A@GMAIL.COM",
				EmailConfirmed = true,
				PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "superpassowrd"),
				SecurityStamp = string.Empty
			});
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
			{
				RoleId = "3c2ffd08-736b-47bd-951b-c3b1cac88240",
				UserId = "20f97c37-e683-4573-aac0-ac2ae70edd49"
			});
			modelBuilder.Entity<TextField>().HasData(new TextField
			{
				Id = new Guid("2629a8f1-879d-4f0c-a183-c012adcaa96a"),
				CodeWord = "PageIndex",
				Title = "Главная"
			});
		}

	}
}
