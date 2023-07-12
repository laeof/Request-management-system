using Microsoft.AspNetCore.Mvc;
using RMS.Domain;
using RMS.Domain.Entities;

namespace RMS.Controllers
{
	public class CategoryController: Controller
	{
		private DataManager dataManager { get; set; }
		public CategoryController(DataManager dataManager) 
		{
			this.dataManager = dataManager;
		}
		public IActionResult Categories()
		{
			return View(dataManager.Categories.GetCategories());
		}
		[HttpGet]
		public IActionResult Create() 
		{
			var category = new Category();
			ViewBag.Title = "Створити категорію";
			return View(category);
		}
        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
			ViewBag.Title = "Створити категорію";

			var existname = dataManager.Categories.GetCategories()
				.Where(x => x.Name.ToLower() == model.Name.ToLower()).FirstOrDefault();

			if(existname != null)
				return View();

			var newcategory = new Category()
			{
				Name = model.Name
			};

			await dataManager.Categories.SaveCategoryAsync(newcategory);

			return RedirectToAction("Categories");
        }
        public IActionResult Edit() 
		{
			return View();
		}
		public async Task<IActionResult> Delete(uint id) 
		{
			await dataManager.Categories.DeleteCategoryAsync(id);
			return RedirectToAction("Categories");
		}
	}
}
