using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Domain;
using System.Data;

namespace RMS.Controllers
{

	[Authorize]
	public class UserController : Controller
	{
		private readonly AppDbContext _db;
		public UserController(AppDbContext db)
		{
			_db = db;
		}
        [HttpGet]
        public IActionResult Users()
        {
            var userRoles = _db.UserRole
                .Include(ur => ur.User)
                .Include(ur => ur.Role)
                .ToList();

            var users = userRoles.Select(ur => ur.User).ToList();

            return View(users);
        }
    }
}
