using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
			return View();
		}
	}
}
