using Microsoft.AspNetCore.Mvc;
using RMS.Domain;
using RMS.Models;

namespace RMS.Components
{
	public class LeftmenuViewComponent: ViewComponent
	{
		private readonly DataManager dataManager;
		public LeftmenuViewComponent(DataManager dataManager)
		{
			this.dataManager = dataManager;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var a = new LeftMenuModel { User = dataManager.Users.GetUserById(Convert.ToUInt32(Request.Cookies["Id"])) };
			return View(a);
		}
	}
}
