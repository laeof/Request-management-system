using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RMS.Domain;
using RMS.Domain.Entities;
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
        private async Task<List<Request>> ShowRequests(int status, int page) 
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
                r.Closed = await dataManager.Users.GetUserByIdAsync(r.CloseId);
                r.Category = await dataManager.Categories.GetCategoryByIdAsync(r.CategoryId);
                r.Lifecycle = await dataManager.Lifecycles.GetLifecycleByIdAsync(r.LifecycleId);
                r.Cancelled = await dataManager.Users.GetUserByIdAsync(r.CancelId);
            }

            ViewBag.PlanningRequestsCount = dataManager.Requests.GetRequestByStatus(1).Count();
            ViewBag.CurrentRequestsCount = dataManager.Requests.GetRequestByStatus(2).Count();
            ViewBag.ClosedRequestsCount = dataManager.Requests.GetRequestByStatus(3).Count();
            ViewBag.CancelledRequestsCount = dataManager.Requests.GetRequestByStatus(4).Count();

            return reqs;
        }
        [HttpGet]
        [Authorize(Roles ="admin, manager")]
        public async Task<IActionResult> Edit(uint id)
        {
            ViewBag.Title = "Редагувати заявку";

            var request = await dataManager.Requests.GetRequestByIdAsync(id);

            request.Categories = dataManager.Categories.GetCategories().ToList();

            return View(request);
        }
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> Edit(Request model)
        {
            model.Lifecycle = await dataManager.Lifecycles.GetLifecycleByIdAsync(model.LifecycleId);

			switch (model.Status)
			{
				case 1:
					model.Lifecycle.Planning = DateTime.UtcNow;
					break;
				case 2:
					model.Lifecycle.Current = DateTime.UtcNow;
					model.OpenId = userManager.User.Id;
					break;
				case 3:
					model.Lifecycle.Closed = DateTime.UtcNow;
					model.CloseId = userManager.User.Id;
					break;
				case 4:
					model.Lifecycle.Cancelled = DateTime.UtcNow;
                    model.CancelId = userManager.User.Id;
					break;
			}

            await dataManager.Requests.SaveRequestAsync(model);

            switch (model.Status)
            {
                case 1:
					return RedirectToAction("PlanningRequests");
				case 2:
					return RedirectToAction("CurrentRequests");
				case 3:
					return RedirectToAction("ClosedRequests");
				case 4:
					return RedirectToAction("CancelledRequests");
                default:
                    return RedirectToAction("Home");
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
                request.OpenId = userManager.User.Id;

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
                request.CloseId = userManager.User.Id;

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
				request.CancelId = userManager.User.Id;

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

            var Request = new Request
            {
                Categories = dataManager.Categories.GetCategories().ToList()
            };
			return View(Request);
		}
		[Authorize(Roles = "admin, manager")]
		[HttpPost]
		public async Task<IActionResult> Create(Request model)
		{
			ViewBag.Title = "Запланувати заявку";

			if (ModelState.IsValid)
			{
                var lifecycle = new Lifecycle()
                {
                    Planning = DateTime.UtcNow
                };

                await dataManager.Lifecycles.SaveLifecycleAsync(lifecycle);

                var currentUser = await dataManager.Users.GetUserByIdAsync(userManager.User.Id);

				// create request
				var request = new Request
                {
                    Name = model.Name,
                    Description = model.Description,
                    Priority = model.Priority,
                    Address = model.Address,
                    CategoryId = model.CategoryId,
                    Comment = model.Comment,
                    CreatedName = currentUser.FirstName
                    + " " + currentUser.Surname,
                    LifecycleId = lifecycle.Id
				};

                // save request to db
                await dataManager.Requests.SaveRequestAsync(request);

				// redirect to requests
				return Redirect("/Request/PlanningRequests");
			}

			ModelState.AddModelError("", "Помилка валідації форми");

			var category = new Request
            {
				Categories = dataManager.Categories.GetCategories().ToList()
			};

			return View(category);
		}
	}
}
