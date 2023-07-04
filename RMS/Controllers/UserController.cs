using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Authorize]
        [HttpGet]
		[Authorize(Roles = "admin, manager")]
		public IActionResult Users()
        {
			ViewBag.Title = "Список облікових записів";
            ViewBag.Id = Convert.ToUInt32(Request.Cookies["Id"]);

            var users = dataManager.UserRole.GetUserRole()
                 .Include(a => a.User)
                 .ToList();

            return View(users);
        }
		[Authorize(Roles = "admin, manager")]
		[HttpGet]
        public IActionResult Edit(uint id)
        {
            var userrole = dataManager.UserRole.GetUserRoleById(id);
            userrole.User = dataManager.Users.GetUserById(userrole.UserId);
            userrole.User.Id = userrole.UserId;
            userrole.Role = dataManager.Role.GetRoleById(userrole.RoleId);
            userrole.UserRoleId = id;
            return View(userrole);
        }
		[Authorize(Roles = "admin, manager")]
		[HttpPost]
        public IActionResult Edit(UserRole userrole)
        {
            if (userrole != null)
            {
                dataManager.Users.SaveUser(userrole.User);
                dataManager.UserRole.SaveUserRole(userrole);
            }

            return RedirectToAction("Users");
        }
		[Authorize(Roles = "admin, manager")]
		public IActionResult ActivityChange(uint id)
        {
            var user = dataManager.Users.GetUserById(id);

            if (user != null)
            {
                user.IsActive = !user.IsActive;
                dataManager.Users.SaveUser(user);
            }
            return RedirectToAction("Users");
        }
		[Authorize(Roles = "admin, manager")]
		public IActionResult Delete(uint id)
        {
            var user = dataManager.Users.GetUserById(id);

            if (user != null)
            {
                dataManager.Users.DeleteUser(user);
            }

            return RedirectToAction("Users");
        }
    }
	
}
