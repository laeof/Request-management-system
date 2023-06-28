using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Domain;
using RMS.Models;
using RMS.Service;
using System.Security.Claims;

namespace RMS.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private readonly AppDbContext _db;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public AccountController(AppDbContext db, IHttpContextAccessor httpContextAccessor)
		{
			_db = db;
			_httpContextAccessor = httpContextAccessor;
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
				uint userID = 0;

				if (Extensions.ValidateUser(model.Login, model.Password, _db, ref userID))
				{

					//користувач

                    var user = await _db.Users
						.FirstOrDefaultAsync(u => u.Login.ToLower() == model.Login.ToLower());

					//роль користувача

                    var userRole = await _db.UserRole
						.Include(ur => ur.Role)
						.FirstOrDefaultAsync(ur => ur.UserId == user.Id);

                    //користувач + роль

                    var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, user.FirstName),
						new Claim(ClaimTypes.Role, userRole.Role.Name)
					};

                    var identity = new ClaimsIdentity(claims, "Auth");
                    var principal = new ClaimsPrincipal(identity);

                    //авторизація

                    Response.Cookies.Append("Id", userID.ToString());

                    await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

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
		[Authorize]
		public IActionResult PersonalPage()
		{
			return View("PersonalPage");
		}
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");
		}
	}
}
