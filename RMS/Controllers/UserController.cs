using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Domain;
using RMS.Domain.Entities;
using RMS.Models;
using System;

namespace RMS.Controllers
{

	[Authorize]
	public class UserController : Controller
	{
        private readonly UserManager userManager;
		private readonly DataManager dataManager;
        private readonly IWebHostEnvironment environment;
        public UserController(DataManager dataManager, UserManager userManager, IWebHostEnvironment environment)
		{
			this.dataManager = dataManager;
            this.userManager = userManager;
            this.environment = environment;
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

            var model = new UserViewModel()
            {
                UserRoles = users
            };

            return View(model);
        }
		[Authorize(Roles = "admin, manager")]
		[HttpGet]
        public async Task<IActionResult> Edit(uint id)
		{
			ViewBag.Title = "Редагування облікових записів";
			ViewBag.Avatar = "Аватар";

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

            var model = new UserViewModel()
            {
                UserId = userrole.UserId,
                UserRoleId = userrole.RoleId,
                RoleId = userrole.RoleId,
                User = userrole.User,
                Role = userrole.Role
            };

            return View(model);
        }
		[Authorize(Roles = "admin, manager")]
		[HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model, IFormFile AvatarFile)
        {
            var userrole = model;

            ViewBag.Avatar = "Аватар";
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

            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(environment.WebRootPath, "img", "Avatar");
                string uniqueFileName = Path.GetRandomFileName() + "_" + AvatarFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await AvatarFile.CopyToAsync(fileStream);
                }

                userrole.User.ImgPath = "../../img/Avatar/" + uniqueFileName;
            }

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

            await dataManager.Users.SoftDeleteUserAsync(user);

            return RedirectToAction("Users");
        }
    }
	
}
