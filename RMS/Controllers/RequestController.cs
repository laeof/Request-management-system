using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RMS.Domain;
using RMS.Domain.Entities;
using RMS.Models;
using RMS.Service;
using System.Drawing.Printing;

namespace RMS.Controllers
{
    [Authorize]
    public class RequestController : Controller
	{
        private readonly UserManager userManager;
		private readonly DataManager dataManager;
		public RequestController(DataManager dataManager, UserManager userManager)
        {
			this.dataManager = dataManager;
            this.userManager = userManager;
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
                CategoryId = request.CategoryId,
                Comment = request.Comment,
                IsDeleted = request.IsDeleted,
                Priority = request.Priority,
                Status = request.Status,
                Categories = dataManager.Categories.GetCategories().ToList()
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
                Categories = dataManager.Categories.GetCategories().ToList()
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> Edit(RequestViewModel model)
        {
            var request = await dataManager.Requests.GetRequestByIdAsync((uint)model.Id);

            request.Lifecycle = await dataManager.Lifecycles.GetLifecycleByIdAsync(request.LifecycleId);

            request.Address = model.Address;
            request.Comment = model.Comment;
            request.Status = (int)model.Status;
            request.CategoryId = (uint)model.CategoryId;
            request.Priority = (int)model.Priority;

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
                throw new Exception("Failed to save the request."); // Уточняем причину исключения
            }
        }
        [Authorize(Roles = "admin, manager")]
		[HttpGet]
        public async Task<IActionResult> Open(uint id)
        {
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
            return RedirectToAction("CurrentRequests");
        }
		[HttpGet]
        public async Task<IActionResult> Close(uint id)
        {
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
			return RedirectToAction("ClosedRequests");
		}
		[Authorize(Roles = "admin, manager")]
		[HttpGet]
        public async Task<IActionResult> Cancel(uint id)
        {
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
			ViewBag.Title = "Запланувати заявку";

            if (ModelState.IsValid)
            {
                var lifecycle = new Lifecycle()
                {
                    Planning = DateTime.UtcNow
                };

                await dataManager.Lifecycles.SaveLifecycleAsync(lifecycle);

                // create request
                var request = new Request
                {
                    Priority = (int)model.Priority,
                    Address = model.Address,
                    CategoryId = (uint)model.CategoryId,
                    Comment = model.Comment,
                    CreatedId = userManager.User.Id,
                    LifecycleId = lifecycle.Id,
                    AbonentUID = model.AbonentUID
                };

                // save request to db
                await dataManager.Requests.SaveRequestAsync(request);

                // redirect to requests
                return Redirect("/Request/PlanningRequests");
            }

			ModelState.AddModelError("", "Помилка валідації форми");

			model = new RequestViewModel
            {
				Categories = dataManager.Categories.GetCategories().ToList()
			};

			return View(model);
		}
	}
}
