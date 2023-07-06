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
			ViewBag.Title = "Редагування облікових записів";

			var userrole = dataManager.UserRole.GetUserRoleById(id);

            if (userrole == null)
                return RedirectToAction("Users");

            var user_role = userrole.RoleId;

            var current_user_role = dataManager.UserRole.GetUserRole()
            .FirstOrDefault(role => role.UserId == Convert.ToUInt32(Request.Cookies["Id"]))
            .RoleId;

            //if current user is manager or less
            if (current_user_role >= 2)
                if (current_user_role > user_role)
                    return RedirectToAction("Users");

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
			ViewBag.Title = "Редагування облікових записів";

			if (userrole == null)
                return RedirectToAction("Users");

            var user_role = userrole.RoleId;

            var current_user_role = dataManager.UserRole.GetUserRole()
            .FirstOrDefault(role => role.UserId == Convert.ToUInt32(Request.Cookies["Id"]))
            .RoleId;

            //if current user is manager or less
            if (current_user_role >= 2)
                if (current_user_role > user_role)
                    return RedirectToAction("Users");

            dataManager.Users.SaveUser(userrole.User);
            dataManager.UserRole.SaveUserRole(userrole);

            return RedirectToAction("Users");
        }
		[Authorize(Roles = "admin, manager")]
		public IActionResult ActivityChange(uint id)
		{
			ViewBag.Title = "Редагування облікових записів";

			var user = dataManager.Users.GetUserById(id); 
            
            if(user == null)
                return RedirectToAction("Users");

            var user_role = dataManager.UserRole.GetUserRole()
            .FirstOrDefault(role => role.UserId == id)
            .RoleId;

            var current_user_role = dataManager.UserRole.GetUserRole()
            .FirstOrDefault(role => role.UserId == Convert.ToUInt32(Request.Cookies["Id"]))
            .RoleId;

            //if current user is manager or less
            if(current_user_role >= 2)
                if (current_user_role > user_role)
                    return RedirectToAction("Users");

            user.IsActive = !user.IsActive;
            dataManager.Users.SaveUser(user);

            return RedirectToAction("Users");
        }
		[Authorize(Roles = "admin, manager")]
		public IActionResult Delete(uint id)
        {
			ViewBag.Title = "Редагування облікових записів";

			var user = dataManager.Users.GetUserById(id);

            if (user == null)
                return RedirectToAction("Users");

            var user_role = dataManager.UserRole.GetUserRole()
            .FirstOrDefault(role => role.UserId == id)
            .RoleId;

            var current_user_role = dataManager.UserRole.GetUserRole()
            .FirstOrDefault(role => role.UserId == Convert.ToUInt32(Request.Cookies["Id"]))
            .RoleId;

            //if current user is manager or less
            if (current_user_role >= 2)
                if (current_user_role > user_role)
                    return RedirectToAction("Users");

            dataManager.Users.DeleteUser(user);

            return RedirectToAction("Users");
        }
    }
	
}
