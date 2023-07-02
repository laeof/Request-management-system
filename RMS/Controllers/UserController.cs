using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Domain;
using RMS.Domain.Entities;

namespace RMS.Controllers
{

	[Authorize]
	public class UserController : Controller
	{
        private readonly DataManager dataManager;
		public UserController(DataManager dataManager)
		{
            this.dataManager = dataManager;
		}
        [HttpGet]
        public IActionResult Users()
        {
			@ViewBag.Title = "Список облікових записів";

            var users = dataManager.Users.GetUsers();

            return View(users);
        }
	}
	
}
