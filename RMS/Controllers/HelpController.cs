using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RMS.Controllers
{
	public class HelpController : Controller
	{
		public HelpController() { }
		[Authorize]
		public IActionResult Help()
		{ 
			ViewBag.Title = "Сторінка допомоги";
			return View();
		}
	}
}
