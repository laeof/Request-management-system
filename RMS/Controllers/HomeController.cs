using Microsoft.AspNetCore.Mvc;

namespace RMS.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return Redirect("Account/Login");
			}
			return View();
		}
	}
}
