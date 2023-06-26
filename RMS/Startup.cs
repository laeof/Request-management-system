﻿namespace RMS
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using System;
	using RMS.Domain;
	using RMS.Domain.Repositories.Abstract;
	using RMS.Domain.Repositories.EntityFramework;
	using RMS.Service;
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration) => Configuration = configuration;
		public void ConfigureServices(IServiceCollection services)
		{
			//appsettings.json
			Configuration.Bind("Project", new Config());

			//add services
			services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
			services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
			services.AddTransient<DataManager>();

			//db context
			services.AddDbContext<AppDbContext>(x => x.UseNpgsql(Config.ConnectionString));

			//identity
			services.AddIdentity<IdentityUser, IdentityRole>(opts =>
			{
				opts.User.RequireUniqueEmail = true;
				opts.Password.RequiredLength = 6;
				opts.Password.RequireNonAlphanumeric = false;
				opts.Password.RequireUppercase = false;
				opts.Password.RequireLowercase = false;
				opts.Password.RequireDigit = false;
			}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

			//cookie
			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.Name = "default";
				options.Cookie.HttpOnly = true;
				options.LoginPath = "/Account/Login";
				options.AccessDeniedPath = "/Account/Accessdenied";
				options.SlidingExpiration = true;
			});

			//auth policy for admin area
			services.AddAuthorization(x =>
			{
				x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
			});

			//add mvc
			services.AddControllersWithViews(x =>
			{
				x.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
			})
				.AddSessionStateTempDataProvider();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//if exceptions
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			//auth and cookie
			app.UseCookiePolicy();
			app.UseAuthentication();
			app.UseAuthorization();

			//static files css html etc
			app.UseStaticFiles();

			//routes
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute("admin", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}