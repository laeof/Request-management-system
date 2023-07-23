using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using RMS.Domain;
using RMS.Domain.Entities;
using RMS.Models;

namespace RMS.Controllers
{
	public class BrigadeController: Controller
	{
		private readonly DataManager dataManager;
		public BrigadeController(DataManager dataManager)
		{
			this.dataManager = dataManager;
		}
		public IActionResult Brigades() 
		{
            BrigadeViewModel brigadeViewModel = new BrigadeViewModel()
            {
                ViewBrigades = dataManager.Brigades.GetBrigades().ToList()
            };
            return View(brigadeViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BrigadeViewModel brigadeViewModel = new BrigadeViewModel()
            {
                Mounters = dataManager.Users.GetUsers().Where(x => x.UserRoles.Any(ur => ur.RoleId > 1)).ToList()
            };
            return View(brigadeViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(BrigadeViewModel brigadeViewModel, List<int> Ids)
        {
			if (Ids != null)
			{
				//save brigade
				await dataManager.Brigades.SaveBrigadeAsync(brigadeViewModel);

				//get mounters
				var selectedMounters = dataManager.Users.GetUsers().Where(u => Ids.Contains((int)u.Id)).ToList();

				foreach (var mounter in selectedMounters)
				{
					var brigadeMounter = new BrigadeMounter
					{
						BrigadeId = brigadeViewModel.Id,
						UserId = mounter.Id
					};

					//save brigademounters
					await dataManager.BrigadeMounter.SaveBrigadeMounterAsync(brigadeMounter);
				}
			}

			return RedirectToAction("Brigades");
        }
    }
}
