using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
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
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
			ViewBag.UserNamePlaceholder = "Логін";
			ViewBag.PasswordPlaceholder = "Пароль";
			ViewBag.FirstNamePlaceholder = "Ім'я";
			ViewBag.SurnamePlaceholder = "Прізвище";
			ViewBag.CommentPlaceholder = "Коментар";
			ViewBag.RoleIdPlaceholder = "Роль";
			if (ModelState.IsValid)
            {
                //check for existing
                var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Login.ToLower() == model.Login.ToLower());
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

                // save user to db
                _db.Users.Add(user);

				await _db.SaveChangesAsync();

				// create role for user
				var userrole = new UserRole
				{
					UserId = user.Id,
					RoleId = model.RoleId,
				};

				//save user role to db
				_db.UserRole.Add(userrole);

                await _db.SaveChangesAsync();

                // redirect to users
                return Redirect("/User/Users");
            }

            // if invalid register again
            return View(model);
        }
    }
}
