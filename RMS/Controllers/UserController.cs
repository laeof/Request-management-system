using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Domain;
using RMS.Domain.Entities;
using System.Security.Claims;

namespace RMS.Controllers
{

	[Authorize]
	public class UserController : Controller
	{
		private readonly uint CurrentUserId = 0;
		private readonly DataManager dataManager;
		private readonly IHttpContextAccessor httpContextAccessor;
		public UserController(DataManager dataManager, IHttpContextAccessor httpContextAccessor)
		{
			this.dataManager = dataManager;
			this.httpContextAccessor = httpContextAccessor;
			this.CurrentUserId = (uint)Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
		}
		[Authorize]
        [HttpGet]
		[Authorize(Roles = "admin, manager")]
		public IActionResult Users()
        {
			ViewBag.Title = "Список облікових записів";
            ViewBag.Id = dataManager.Users.GetUserById(CurrentUserId).Id;

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
                .FirstOrDefault(role => role.UserId == CurrentUserId).RoleId;

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
			.AsNoTracking()
            .FirstOrDefault(role => role.UserId == CurrentUserId)
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
            .FirstOrDefault(role => role.UserId == CurrentUserId)
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
            .FirstOrDefault(role => role.UserId == CurrentUserId)
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
