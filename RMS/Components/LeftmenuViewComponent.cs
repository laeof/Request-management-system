using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RMS.Domain;
using RMS.Models;
using System.Security.Claims;

namespace RMS.Components
{
	public class LeftmenuViewComponent: ViewComponent
	{
		private readonly DataManager dataManager;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly uint CurrentUserId = 0;
		public LeftmenuViewComponent(DataManager dataManager, IHttpContextAccessor httpContextAccessor)
		{
			this.dataManager = dataManager;
			this.httpContextAccessor = httpContextAccessor;
			this.CurrentUserId = (uint)Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(new LeftMenuModel { User = dataManager.Users.GetUserById(CurrentUserId) });
		}
	}
}
