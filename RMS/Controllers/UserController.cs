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
        private readonly UserManager userManager;
		private readonly DataManager dataManager;
		public UserController(DataManager dataManager, UserManager userManager)
		{
			this.dataManager = dataManager;
            this.userManager = userManager;
		}
		[Authorize]
        [HttpGet]
		[Authorize(Roles = "admin, manager")]
		public IActionResult Users()
        {
			ViewBag.Title = "Список облікових записів";
            ViewBag.Id = userManager.User.Id;

			var users = dataManager.UserRole.GetUserRole()
                 .Include(a => a.User)
                 .ToList();

            return View(users);
        }
		[Authorize(Roles = "admin, manager")]
		[HttpGet]
        public async Task<IActionResult> Edit(uint id)
		{
			ViewBag.Title = "Редагування облікових записів";

			var userrole = await dataManager.UserRole.GetUserRoleByIdAsync(id);

            if (userrole == null)
                return RedirectToAction("Users");

            var user_role = userrole.RoleId;

            var current_user_role = await dataManager.UserRole.GetUserRole()
                .FirstOrDefaultAsync(role => role.UserId == userManager.User.Id);

            //if current user is manager or less
            if (current_user_role.RoleId >= 2)
                if (current_user_role.RoleId > user_role)
                    return RedirectToAction("Users");

            userrole.User = await dataManager.Users.GetUserByIdAsync(userrole.UserId);

            userrole.User.Id = userrole.UserId;

            userrole.Role = await dataManager.Role.GetRoleByIdAsync(userrole.RoleId);

            userrole.UserRoleId = id;

            return View(userrole);
        }
		[Authorize(Roles = "admin, manager")]
		[HttpPost]
        public async Task<IActionResult> Edit(UserRole userrole)
        {
			ViewBag.Title = "Редагування облікових записів";

			if (userrole == null)
                return RedirectToAction("Users");

            var user_role = userrole.RoleId;

            var current_user_role = await dataManager.UserRole.GetUserRole()
            .FirstOrDefaultAsync(role => role.UserId == userManager.User.Id);

            //if current user is manager or less
            if (current_user_role.RoleId >= 2)
                if (current_user_role.RoleId > user_role)
                    return RedirectToAction("Users");

            await dataManager.Users.SaveUserAsync(userrole.User);
            await dataManager.UserRole.SaveUserRoleAsync(userrole);

            return RedirectToAction("Users");
        }
		[Authorize(Roles = "admin, manager")]
		public async Task<IActionResult> ActivityChange(uint id)
		{
			ViewBag.Title = "Редагування облікових записів";

			var user = await dataManager.Users.GetUserByIdAsync(id); 
            
            if(user == null)
                return RedirectToAction("Users");

            var user_role = await dataManager.UserRole.GetUserRole()
            .FirstOrDefaultAsync(role => role.UserId == id);

            var current_user_role = await dataManager.UserRole.GetUserRole()
            .FirstOrDefaultAsync(role => role.UserId == userManager.User.Id);

            //if current user is manager or less
            if(current_user_role.RoleId >= 2)
                if (current_user_role.RoleId > user_role.RoleId)
                    return RedirectToAction("Users");

            user.IsActive = !user.IsActive;
            await dataManager.Users.SaveUserAsync(user);

            return RedirectToAction("Users");
        }
		[Authorize(Roles = "admin, manager")]
		public async Task<IActionResult> Delete(uint id)
        {
			ViewBag.Title = "Редагування облікових записів";

			var user = await dataManager.Users.GetUserByIdAsync(id);

            if (user == null)
                return RedirectToAction("Users");

            var user_role = await dataManager.UserRole.GetUserRole()
            .FirstOrDefaultAsync(role => role.UserId == id);

            var current_user_role = await dataManager.UserRole.GetUserRole()
            .FirstOrDefaultAsync(role => role.UserId == userManager.User.Id);

            //if current user is manager or less
            if (current_user_role.RoleId >= 2)
                if (current_user_role.RoleId > user_role.RoleId)
                    return RedirectToAction("Users");

            await dataManager.Users.DeleteUserAsync(user);

            return RedirectToAction("Users");
        }
    }
	
}
