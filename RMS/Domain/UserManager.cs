using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using System.Security.Claims;
using RMS.Models;

namespace RMS.Domain
{
	public class UserManager
	{
		private readonly DataManager dataManager;
		private readonly IHttpContextAccessor httpContextAccessor;
		public UserManager(DataManager dataManager, IHttpContextAccessor httpContextAccessor)
		{
			this.dataManager = dataManager;
			this.httpContextAccessor = httpContextAccessor;
		}

		public async Task<bool> SignInAsync(string login)
		{

			var User = await dataManager.Users.GetUsers().FirstOrDefaultAsync(u => u.Login.ToLower() == login.ToLower());

			var Role = dataManager.Role.GetRoleById(
				dataManager.UserRole.GetUserRole().FirstOrDefault(ur => ur.UserId == User.Id).RoleId);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, User.Id.ToString()),
				new Claim(ClaimTypes.Name, User.Login),
				new Claim(ClaimTypes.Role, Role.Name)
			};

			var identity = new ClaimsIdentity(claims, "Auth");
			var principal = new ClaimsPrincipal(identity);

			await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

			return true;
		}

		public async Task<bool> SignOutAsync()
		{
			await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return true;
		}

		public async Task<bool> SingUpAsync(RegisterViewModel model)
		{
			// create user
			var user = new User
			{
				Login = model.Login,
				Password = model.Password,
				FirstName = model.FirstName,
				Surname = model.Surname,
				Comment = model.Comment
			};

			//save user
			dataManager.Users.SaveUser(user);

			// create role for user
			var userrole = new UserRole
			{
				UserId = user.Id,
				RoleId = model.RoleId,
			};

			//save user role to db

			dataManager.UserRole.SaveUserRole(userrole);

			return true;
		}
	}
}
