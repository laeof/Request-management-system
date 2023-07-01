namespace RMS
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using System;
	using RMS.Domain;
	using RMS.Domain.Repositories.Abstract;
	using RMS.Domain.Repositories.EntityFramework;
	using RMS.Service;
	using RMS.Models;
	using Microsoft.AspNetCore.Authentication.Cookies;

	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration) => Configuration = configuration;
		public void ConfigureServices(IServiceCollection services)
		{
			//appsettings.json
			Configuration.Bind("Project", new Config());

			//add services
			services.AddTransient<IRequestRepository, EFRequestRepository>();
			services.AddTransient<ICategoryRepository, EFCategoryRepository>();
			services.AddTransient<ILifecycleRepository, EFLifecycleRepository>();
			services.AddTransient<IUserRepository, EFUserRepository>();
			services.AddTransient<IUserRoleRepository, EFUserRoleRepository>();
			services.AddTransient<IRoleRepository, EFRoleRepository>();
			services.AddTransient<DataManager>();

			//db context
			services.AddDbContext<AppDbContext>(x => x.UseNpgsql(Config.ConnectionString));

			//cookie
			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.Name = "default";
				options.Cookie.HttpOnly = true;
				options.LoginPath = "/Account/Login";
				options.AccessDeniedPath = "/Home";
				options.SlidingExpiration = true;
			});

			//auth policy for admin area
			services.AddAuthorization(x =>
			{
				x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
			});

			services.AddAuthentication(options =>
			{
				options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			}).AddCookie();

			services.AddHttpContextAccessor();

			//add mvc
			services.AddControllersWithViews()
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