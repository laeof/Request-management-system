using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Domain;
using RMS.Domain.Entities;
using Microsoft.Extensions.Logging;
using RMS.Models;
using RMS.Service;
using System.Drawing.Printing;
using System.Net;

namespace RMS.Controllers
{
    [Authorize]
    public class RequestController : Controller
	{
        private readonly UserManager userManager;
		private readonly DataManager dataManager;
		private readonly ILogger<RequestController> logger;
		public RequestController(DataManager dataManager, UserManager userManager, ILogger<RequestController> logger)
        {
			this.dataManager = dataManager;
            this.userManager = userManager;
            this.logger = logger;
		}

        private const int PageSize = 10;
        [HttpGet]
        public async Task<IActionResult> PlanningRequests(int page = 1)
        {
            ViewBag.Title = "Заплановані заявки";
            return View(await ShowRequests(1, page));
        }
        [HttpGet]
        public async Task<IActionResult> CurrentRequests(int page = 1)
        {
            ViewBag.Title = "Поточні заявки";
            return View(await ShowRequests(2, page));
        }
        [HttpGet]
        public async Task<IActionResult> ClosedRequests(int page = 1)
        {
            ViewBag.Title = "Закриті заявки";
            return View(await ShowRequests(3, page));
        }
        [HttpGet]
        public async Task<IActionResult> CancelledRequests(int page = 1)
        {
            ViewBag.Title = "Відмінені заявки";
            return View(await ShowRequests(4, page));
        }
        private async Task<RequestViewModel> ShowRequests(int status, int page) 
        {
            var requests = dataManager.Requests.GetRequestByStatus(status);

            switch (status)
            {
                case 1:
                    requests = requests.OrderByDescending(x => x.Priority).ThenByDescending(x => x.Lifecycle.Planning);
                    break;
                case 2:
                    requests = requests.OrderByDescending(x => x.Priority).ThenByDescending(x => x.Lifecycle.Current);
                    break;
                case 3:
                    requests = requests.OrderByDescending(x => x.Priority).ThenByDescending(x => x.Lifecycle.Closed);
                    break;
                case 4:
                    requests = requests.OrderByDescending(x => x.Priority).ThenByDescending(x => x.Lifecycle.Cancelled);
                    break;
            }

            var totalItems = requests.Count();

            var reqs = requests
                .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            requests = null;

            foreach (var r in reqs)
            {
                r.Close = await dataManager.Users.GetUserByIdAsync(r.ClosedId);
                r.Category = await dataManager.Categories.GetCategoryByIdAsync(r.CategoryId);
                r.Lifecycle = await dataManager.Lifecycles.GetLifecycleByIdAsync(r.LifecycleId);
                r.Cancel = await dataManager.Users.GetUserByIdAsync(r.CancelledId);
            }

            var model = new RequestViewModel
            {
                Requests = reqs
            };

            ViewBag.PlanningRequestsCount = dataManager.Requests.GetRequestByStatus(1).Count();
            ViewBag.CurrentRequestsCount = dataManager.Requests.GetRequestByStatus(2).Count();
            ViewBag.ClosedRequestsCount = dataManager.Requests.GetRequestByStatus(3).Count();
            ViewBag.CancelledRequestsCount = dataManager.Requests.GetRequestByStatus(4).Count();

            return model;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Info(uint id)
		{
			ViewBag.Title = "Інформація про заявку";

            var request = await dataManager.Requests.GetRequestByIdAsync(id);

            var model = new RequestViewModel
            {
                Id = id,
                Address = request.Address,
                Comment = request.Comment,
                IsDeleted = request.IsDeleted,
                Priority = request.Priority,
                Status = request.Status,
                AbonentUID = request.AbonentUID,
                Category = await dataManager.Categories.GetCategoryByIdAsync(request.CategoryId)
            };

            return View(model);
        }
        [HttpGet]
        [Authorize(Roles ="admin, manager")]
        public async Task<IActionResult> Edit(uint id)
        {
            ViewBag.Title = "Редагувати заявку";

            var request = await dataManager.Requests.GetRequestByIdAsync(id);

            var model = new RequestViewModel {
                Id = id,
                Comment = request.Comment,
                Status = request.Status,
                Priority = request.Priority,
                Address = request.Address,
                CategoryId= request.CategoryId,
                Categories = dataManager.Categories.GetCategories().ToList(),
                AbonentUID = request.AbonentUID
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> Edit(RequestViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    logger.LogInformation($"Changing a request. UserId: {userManager.User.Id}. RequestId: {model.Id}");

                    var request = await dataManager.Requests.GetRequestByIdAsync((uint)model.Id);

                    request.Lifecycle = await dataManager.Lifecycles.GetLifecycleByIdAsync(request.LifecycleId);

                    request.Address = model.Address;
                    request.Comment = model.Comment;
                    request.Status = (int)model.Status;
                    request.Priority = (int)model.Priority;
                    request.AbonentUID = model.AbonentUID;

					if (model.CategoryId == 0)
					{
						if (model.Category.Name == null)
							throw new DbUpdateException("Category name is null");

						var newCategory = new Category
						{
							Name = model.Category.Name
						};

						// Save the new category to the database
						await dataManager.Categories.SaveCategoryAsync(newCategory);
                        
                        request.CategoryId = newCategory.Id;

					}
					else
					{
						request.CategoryId = (uint)model.CategoryId;
					}

					switch (model.Status)
                    {
                        case 1:
                            request.Lifecycle.Planning = DateTime.UtcNow;
                            request.Lifecycle.Current = null;
                            request.Lifecycle.Closed = null;
                            request.Lifecycle.Cancelled = null;
                            break;
                        case 2:
                            request.Lifecycle.Current = DateTime.UtcNow;
                            request.OpenedId = userManager.User.Id;
                            request.Lifecycle.Closed = null;
                            request.Lifecycle.Cancelled = null;
                            break;
                        case 3:
                            request.Lifecycle.Closed = DateTime.UtcNow;
                            request.ClosedId = userManager.User.Id;
                            request.Lifecycle.Cancelled = null;
                            break;
                        case 4:
                            request.Lifecycle.Cancelled = DateTime.UtcNow;
                            request.CancelledId = userManager.User.Id;
                            request.Lifecycle.Closed = null;
                            break;
                    }

                    request.IsDeleted = model.IsDeleted;

                    if (await dataManager.Requests.SaveRequestAsync(request))
                    {

                        logger.LogInformation($"Request has been changed. UserId: {userManager.User.Id}. RequestId: {model.Id}");

                        switch (model.Status)
                        {
                            case 1:
                                return RedirectToAction(nameof(PlanningRequests));
                            case 2:
                                return RedirectToAction(nameof(CurrentRequests));
                            case 3:
                                return RedirectToAction(nameof(ClosedRequests));
                            case 4:
                                return RedirectToAction(nameof(CancelledRequests));
                            default:
                                return RedirectToAction("Home");
                        }
                    }
                    else
                    {
                        throw new DbUpdateException("Failed to save the request.");
                    }
                }
				else
				{
					throw new DbUpdateException("ModelState is not valid.");
				}
			}
			catch (DbUpdateException ex)
			{
                logger.LogError($"Unable to save database. UserId: {userManager.User.Id}. More info: {ex.Message}");

				var request = await dataManager.Requests.GetRequestByIdAsync(model.Id);

				var requestmodel = new RequestViewModel
				{
					Id = request.Id,
					Comment = request.Comment,
					Status = request.Status,
					Priority = request.Priority,
					Address = request.Address,
					CategoryId = request.CategoryId,
					Categories = dataManager.Categories.GetCategories().ToList(),
					AbonentUID = request.AbonentUID
				};

				return View(requestmodel);
			}
		}
        [Authorize(Roles = "admin, manager")]
		[HttpGet]
        public async Task<IActionResult> Open(uint id)
        {

			logger.LogInformation($"Opening a request. UserId: {userManager.User.Id}. RequestId: {id}");

			var request = await dataManager.Requests.GetRequestByIdAsync(id);

            if (request != null)
            {
                //кто відкрив
                request.OpenedId = userManager.User.Id;

                //життєвий цикл заявки
                request.Lifecycle = await dataManager.Lifecycles.GetLifecycleByIdAsync(request.LifecycleId);
                request.Lifecycle.Current = DateTime.UtcNow;

                //заявка відкрита
                request.Status = 2;

                //зберегти
                await dataManager.Requests.SaveRequestAsync(request);
            }

			logger.LogInformation($"Request has been opened. UserId: {userManager.User.Id}. RequestId: {id}");

			return RedirectToAction("CurrentRequests");
        }
		[HttpGet]
        public async Task<IActionResult> Close(uint id)
        {
			logger.LogInformation($"Closing a request. UserId: {userManager.User.Id}. RequestId: {id}");

			var request = await dataManager.Requests.GetRequestByIdAsync(id);

			if (request != null)
			{
                //кто закрив
                request.ClosedId = userManager.User.Id;

				//життєвий цикл заявки
				request.Lifecycle = await dataManager.Lifecycles.GetLifecycleByIdAsync(request.LifecycleId);
				request.Lifecycle.Closed = DateTime.UtcNow;

				//заявка закрита
				request.Status = 3;

				//зберегти
				await dataManager.Requests.SaveRequestAsync(request);
			}

			logger.LogInformation($"Request has been closed. UserId: {userManager.User.Id}. RequestId: {id}");

			return RedirectToAction("ClosedRequests");
		}
		[Authorize(Roles = "admin, manager")]
		[HttpGet]
        public async Task<IActionResult> Cancel(uint id)
        {
			logger.LogInformation($"Cancelling a request. UserId: {userManager.User.Id}. RequestId: {id}");

			var request = await dataManager.Requests.GetRequestByIdAsync(id);

			if (request != null)
			{
				//кто відмінив
				request.CancelledId = userManager.User.Id;

				//життєвий цикл заявки
				request.Lifecycle = await dataManager.Lifecycles.GetLifecycleByIdAsync(request.LifecycleId);
				request.Lifecycle.Cancelled = DateTime.UtcNow;

				//заявка відмінена
				request.Status = 4;

				//зберегти
				await dataManager.Requests.SaveRequestAsync(request);
			}

			logger.LogInformation($"Request has been cancelled. UserId: {userManager.User.Id}. RequestId: {id}");

			return RedirectToAction("CancelledRequests");
		}
		[Authorize(Roles = "admin, manager")]
		[HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Запланувати заявку";

            var model = new RequestViewModel()
            {
                Categories = dataManager.Categories.GetCategories().ToList()
			};

			return View(model);
		}
		[Authorize(Roles = "admin, manager")]
		[HttpPost]
		public async Task<IActionResult> Create(RequestViewModel model)
		{
            try
            {
				logger.LogInformation($"Creating a request. UserId: {userManager.User.Id}");

				ViewBag.Title = "Запланувати заявку";

                if (ModelState.IsValid)
                {
                    var lifecycle = new Lifecycle();

                    switch (model.Status)
                    {
                        case 1:
                            lifecycle.Planning = DateTime.UtcNow;
                            break;
                        case 2:
                            lifecycle.Planning = DateTime.UtcNow;
                            lifecycle.Current = DateTime.UtcNow;
                            model.OpenedId = userManager.User.Id;
                            break;
                    }

                    await dataManager.Lifecycles.SaveLifecycleAsync(lifecycle);

                    var request = new Request();

                    // create request

                    if (model.CategoryId == 0)
                    {
                        if (model.Category.Name == null)
                            throw new DbUpdateException("Category name is null");

						var newCategory = new Category
                        {
                            Name = model.Category.Name
                        };

                        // Save the new category to the database
                        await dataManager.Categories.SaveCategoryAsync(newCategory);

                        request = new Request
                        {
                            CategoryId = newCategory.Id,
                        };
                    }
                    else
                    {
                        request = new Request
                        {
                            CategoryId = (uint)model.CategoryId,
                        };
                    }

                    request.Priority = (int)model.Priority;
                    request.Address = model.Address;
                    request.Comment = model.Comment;
                    request.CreatedId = userManager.User.Id;
                    request.LifecycleId = lifecycle.Id;
                    request.Status = model.Status;
                    request.AbonentUID = model.AbonentUID;

					// save request to db
					await dataManager.Requests.SaveRequestAsync(request);

					logger.LogInformation($"Request with id: {request.Id} has been created. UserId: {userManager.User.Id}");

					// redirect to requests
					switch (model.Status)
                    {
                        case 1:
                            return Redirect("/Request/PlanningRequests");
                        case 2:
                            return Redirect("/Request/CurrentRequests");
                    }

                }

                //validation error

                ModelState.AddModelError("", "Помилка валідації форми");

                model = new RequestViewModel
                {
                    Categories = dataManager.Categories.GetCategories().ToList()
                };

				logger.LogError($"Unable to create a request cause of validation error. UserId: {userManager.User.Id}");

                return View(model);
            }
            catch (DbUpdateException ex)
			{
                //save db error

				logger.LogError($"Unable to save database. UserId: {userManager.User.Id}. More info: {ex.Message}");

				ModelState.AddModelError("", "Невірно введено дані");

				model = new RequestViewModel
				{
					Categories = dataManager.Categories.GetCategories().ToList()
				};

				return View(model);
			}
		}
	}
}
