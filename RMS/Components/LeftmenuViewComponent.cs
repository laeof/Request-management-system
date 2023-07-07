using Microsoft.AspNetCore.Mvc;
using RMS.Domain;
using RMS.Models;

namespace RMS.Components
{
	public class LeftmenuViewComponent: ViewComponent
	{
		private readonly UserManager userManager;
		public LeftmenuViewComponent(UserManager userManager)
		{
			this.userManager = userManager;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(new LeftMenuModel { User = userManager.User });
		}
	}
}
