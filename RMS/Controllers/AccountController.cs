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
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RMS.Controllers
{
	public class AccountController : Controller
	{
		private readonly DataManager dataManager;
		private readonly UserManager userManager;
		public AccountController( DataManager dataManager, UserManager userManager)
		{
			this.dataManager = dataManager;
			this.userManager = userManager;
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
				if (Extensions.ValidateUser(model.Login, model.Password, dataManager))
				{
					await userManager.SignInAsync(model.Login);

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
			ViewBag.Title = "Персональна сторінка";
			var model = new PersonalPageModel();
			model.User = userManager.User;
			return View(model);
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> PersonalPage(PersonalPageModel model)
		{
			ViewBag.Title = "Персональна сторінка";
			await dataManager.Users.SaveUserAsync(model.User);
			return View(model);
		}

		[Authorize]
		public async Task<IActionResult> Logout()
		{
			ViewBag.Title = "Вихід";
			await userManager.SignOutAsync();
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
        public async Task<IActionResult> Register(RegisterViewModel model)
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
				var existingUser = await dataManager.Users.GetUsers().FirstOrDefaultAsync(u => u.Login.ToLower() == model.Login.ToLower());
				if (existingUser != null)
				{
					ModelState.AddModelError(string.Empty, "Користувач з таким логіном вже існує");
					return View(model);
				}

				await userManager.SingUpAsync(model);

				// redirect to users
				return Redirect("/User/Users");
            }

            // if invalid register again
            return View(model);
        }
    }
}
