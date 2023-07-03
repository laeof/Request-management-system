using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using RMS.Domain;
using RMS.Models;
using RMS.Service;
using System.Security.Claims;

namespace RMS.Controllers
{
	public class AccountController : Controller
	{
		private readonly DataManager dataManager;
		private readonly IHttpContextAccessor httpContextAccessor;
		public AccountController(IHttpContextAccessor httpContextAccessor, DataManager dataManager)
		{
            this.httpContextAccessor = httpContextAccessor;
			this.dataManager = dataManager;
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Login(string returnUrl)
		{
			ViewBag.Title = "Вхід";
			ViewBag.UserNamePlaceholder = "Логін";
			ViewBag.PasswordPlaceholder = "Пароль";
			if (returnUrl == null)
			{
				returnUrl = "/";
			}
			if (User.Identity.IsAuthenticated)
			{
				return Redirect("/Account/PersonalPage");
			}
			ViewBag.ReturnUrl = returnUrl;
			return View(new LoginViewModel());
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				uint userID = 0;

				if (Extensions.ValidateUser(model.Login, model.Password, dataManager, ref userID))
				{
					//користувач

					var User = await dataManager.Users.GetUsers().FirstOrDefaultAsync(u => u.Login.ToLower() == model.Login.ToLower());

					//роль користувача

					var Role = dataManager.Role.GetRoleById(
						dataManager.UserRole.GetUserRole().FirstOrDefault(ur => ur.UserId == userID).RoleId);

                    //користувач + роль

                    var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, User.Login),
						new Claim(ClaimTypes.Role, Role.Name)
					};

                    var identity = new ClaimsIdentity(claims, "Auth");
                    var principal = new ClaimsPrincipal(identity);

                    //авторизація

                    Response.Cookies.Append("Id", userID.ToString());

                    await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    //редіректи

                    return Redirect(returnUrl);

				}
				model.ErrorMessage = "Невірний логін або пароль";
				ViewBag.ReturnUrl = returnUrl;
			}
			ViewBag.Title = "Вхід";
			ViewBag.UserNamePlaceholder = "Логін";
			ViewBag.PasswordPlaceholder = "Пароль";
			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}
		[HttpGet]
		[Authorize]
		public IActionResult PersonalPage()
		{
			var model = new PersonalPageModel();
			model.User = dataManager.Users.GetUserById(Convert.ToUInt32(Request.Cookies["Id"]));
			return View(model);
		}
		[HttpPost]
		[Authorize]
		public IActionResult PersonalPage(PersonalPageModel model)
		{
			dataManager.Users.SaveUser(model.User);
			return View(model);
		}

		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");
		}

		[HttpGet]
		[Authorize(Roles = "admin, manager")]
		public IActionResult Register()
		{
			ViewBag.Title = "Створити обліковий запис";
			ViewBag.UserNamePlaceholder = "Логін";
			ViewBag.PasswordPlaceholder = "Пароль";
			ViewBag.FirstNamePlaceholder = "Ім'я";
			ViewBag.SurnamePlaceholder = "Прізвище";
			ViewBag.CommentPlaceholder = "Коментар";
			ViewBag.RoleIdPlaceholder = "Роль";
			return View(new RegisterViewModel());
		}

		[HttpPost]
        [Authorize(Roles = "admin, manager")]
        public IActionResult Register(RegisterViewModel model)
		{
			ViewBag.Title = "Створити обліковий запис";
			ViewBag.UserNamePlaceholder = "Логін";
			ViewBag.PasswordPlaceholder = "Пароль";
			ViewBag.FirstNamePlaceholder = "Ім'я";
			ViewBag.SurnamePlaceholder = "Прізвище";
			ViewBag.CommentPlaceholder = "Коментар";
			ViewBag.RoleIdPlaceholder = "Роль";

			if (ModelState.IsValid)
            {
                //check for existing
                var existingUser = dataManager.Users.GetUsers().FirstOrDefault(u => u.Login.ToLower() == model.Login.ToLower());
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Користувач з таким логіном вже існує");
                    return View(model);
				}

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

                // redirect to users
                return Redirect("/User/Users");
            }

            // if invalid register again
            return View(model);
        }
    }
}
