using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Domain;
using RMS.Models;

namespace RMS.Controllers
{
	public class HomeController : Controller
	{
		[AllowAnonymous]
		public IActionResult AnonIndex() 
		{
            return RedirectToAction("Login", "Account");
        }

		[Authorize]
		public IActionResult Index()
		{
			ViewBag.Title = "Домашня сторінка";
			return View();
		}
	}
}
