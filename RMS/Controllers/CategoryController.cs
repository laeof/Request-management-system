using Microsoft.AspNetCore.Mvc;
using RMS.Domain;
using RMS.Domain.Entities;
using RMS.Models;

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
			var categories = new CategoryViewModel()
			{
				Categories = dataManager.Categories.GetCategories().ToList()
			};
			return View(categories);
		}
		[HttpGet]
		public IActionResult Create() 
		{
			var category = new CategoryViewModel();
			ViewBag.Title = "Створити категорію";
			return View(category);
		}
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
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
		[HttpGet]
        public IActionResult Edit() 
		{
            var category = new CategoryViewModel();
            ViewBag.Title = "Редагувати категорію";
            return View(category);
        }
		[HttpPost]
		public async Task<IActionResult> Edit(CategoryViewModel model)
		{
            ViewBag.Title = "Редагувати категорію";

            var existname = dataManager.Categories.GetCategories()
                .Where(x => x.Name.ToLower() == model.Name.ToLower()).FirstOrDefault();

            if (existname != null)
                return View();

			var category = await dataManager.Categories.GetCategoryByIdAsync(model.Id);

			if(model.IsDeleted != category.IsDeleted)
				category.IsDeleted = model.IsDeleted;

			category.Name = model.Name;

            await dataManager.Categories.SaveCategoryAsync(category);

            return RedirectToAction("Categories");
        }
		public async Task<IActionResult> Delete(uint id) 
		{
			await dataManager.Categories.SoftDeleteCategoryAsync(id);
			return RedirectToAction("Categories");
		}
	}
}
