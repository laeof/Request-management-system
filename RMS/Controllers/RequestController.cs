using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Domain;
using RMS.Domain.Entities;

namespace RMS.Controllers
{
    [Authorize]
    public class RequestController : Controller
	{
		private readonly DataManager dataManager;
		public RequestController(DataManager dataManager)
        {
			this.dataManager = dataManager;
	    }

        [HttpGet]
        public IActionResult PlanningRequests()
        {
            return View(ShowRequests(1));
        }

        [HttpGet]
        public IActionResult CurrentRequests()
        {
            return View(ShowRequests(2));
        }

        [HttpGet]
        public IActionResult ClosedRequests()
        {
            return View(ShowRequests(3));
        }

        [HttpGet]
        public IActionResult CancelledRequests()
        {
            return View(ShowRequests(4));
        }

        private List<Request> ShowRequests(int status) 
        {
            var requests = dataManager.Requests.GetRequestByStatus(status).ToList();

            if (dataManager == null
                || dataManager.Requests == null
                || dataManager.Users == null
                || dataManager.Categories == null
                || dataManager.Lifecycles == null)
            {
                return requests;
            }

            foreach (var r in requests)
            {
                r.Closed = dataManager.Users.GetUserById(r.CloseId);
                r.Category = dataManager.Categories.GetCategoryById(r.CategoryId);
                r.Lifecycle = dataManager.Lifecycles.GetLifecycleById(r.LifecycleId);
                r.Cancelled = dataManager.Users.GetUserById(r.CancelId);
            }

            ViewBag.PlanningRequestsCount = dataManager.Requests.GetRequestByStatus(1).Count();
            ViewBag.CurrentRequestsCount = dataManager.Requests.GetRequestByStatus(2).Count();
            ViewBag.ClosedRequestsCount = dataManager.Requests.GetRequestByStatus(3).Count();
            ViewBag.CancelledRequestsCount = dataManager.Requests.GetRequestByStatus(4).Count();

            return requests;
        }
		[Authorize(Roles = "admin, manager")]
		[HttpGet]
        public IActionResult Open(uint id)
        {
            var request = dataManager.Requests.GetRequestById(id);

            if (request != null)
            {
                //кто відкрив
                request.OpenId = Convert.ToUInt32(Request.Cookies["Id"]);

                //життєвий цикл заявки
                request.Lifecycle = dataManager.Lifecycles.GetLifecycleById(request.LifecycleId);
                request.Lifecycle.Current = DateTime.UtcNow;

                //заявка відкрита
                request.Status = 2;

                //зберегти
                dataManager.Requests.SaveRequest(request);
            }
            return RedirectToAction("CurrentRequests");
        }
        [HttpGet]
        public IActionResult Close(uint id)
        {
			var request = dataManager.Requests.GetRequestById(id);

			if (request != null)
			{
                //кто закрив
                request.CloseId = Convert.ToUInt32(Request.Cookies["Id"]);

				//життєвий цикл заявки
				request.Lifecycle = dataManager.Lifecycles.GetLifecycleById(request.LifecycleId);
				request.Lifecycle.Closed = DateTime.UtcNow;

				//заявка закрита
				request.Status = 3;

				//зберегти
				dataManager.Requests.SaveRequest(request);
			}
			return RedirectToAction("ClosedRequests");
		}
		[Authorize(Roles = "admin, manager")]
		[HttpGet]
        public IActionResult Cancel(uint id)
        {
			var request = dataManager.Requests.GetRequestById(id);

			if (request != null)
			{
				//кто відмінив
				request.CancelId = Convert.ToUInt32(Request.Cookies["Id"]);

				//життєвий цикл заявки
				request.Lifecycle = dataManager.Lifecycles.GetLifecycleById(request.LifecycleId);
				request.Lifecycle.Cancelled = DateTime.UtcNow;

				//заявка відмінена
				request.Status = 4;

				//зберегти
				dataManager.Requests.SaveRequest(request);
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
		public IActionResult Create(Request model)
		{
			ViewBag.Title = "Запланувати заявку";

			if (ModelState.IsValid)
			{
                var lifecycle = new Lifecycle()
                {
                    Planning = DateTime.UtcNow
                };

                dataManager.Lifecycles.SaveLifecycle(lifecycle);

                // create request
                var request = new Request
                {
                    Name = model.Name,
                    Description = model.Description,
                    Priority = model.Priority,
                    Address = model.Address,
                    CategoryId = model.CategoryId,
                    Comment = model.Comment,
                    CreatedName = dataManager.Users.GetUserById(Convert.ToUInt32(Request.Cookies["Id"])).FirstName 
                    + " " + dataManager.Users.GetUserById(Convert.ToUInt32(Request.Cookies["Id"])).Surname,
                    LifecycleId = lifecycle.Id
				};

                // save request to db
                dataManager.Requests.SaveRequest(request);

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
