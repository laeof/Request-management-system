using Microsoft.AspNetCore.Mvc;
using RMS.Domain;
using RMS.Domain.Entities;
using RMS.Service;

namespace RMS.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class TextFieldsController : Controller
	{
		private readonly DataManager dataManager;
		public TextFieldsController(DataManager dataManager)
		{
			this.dataManager = dataManager;
		}

		public IActionResult Edit(string codeWord)
		{
			var entity = dataManager.TextFields.GetTextFieldByCodeWord(codeWord);
			return View(entity);
		}

		[HttpPost]
		public IActionResult Edit(TextField model)
		{
			if (ModelState.IsValid)
			{
				dataManager.TextFields.SaveTextField(model);
				return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
			}
			return View(model);

        }
	}
}
