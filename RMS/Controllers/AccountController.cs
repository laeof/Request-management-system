using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RMS.Models;

namespace RMS.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;
		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

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
				returnUrl = "/Account/PersonalPage";
				return Redirect(returnUrl);
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
				IdentityUser user = await userManager.FindByNameAsync(model.UserName);
				if (user != null)
				{
					await signInManager.SignOutAsync();
					Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
					if (result.Succeeded)
					{
						return Redirect(returnUrl ?? "/");
					}
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
		[Authorize]
		public IActionResult PersonalPage()
		{
			return View("PersonalPage");
		}
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
