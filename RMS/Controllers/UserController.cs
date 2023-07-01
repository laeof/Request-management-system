using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Domain;

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
            var users = dataManager.Users.GetUsers();

            return View(users);
        }
    }
}
