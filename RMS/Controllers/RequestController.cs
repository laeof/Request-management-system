using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMS.Domain;
using RMS.Models;
using System.Data;

namespace RMS.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly AppDbContext _db;
        public RequestController(AppDbContext db)
        {
            _db = db;

        }
        [HttpGet]
        public IActionResult PlanningRequests()
        {
            var requests = _db.Requests
               .Include(ur => ur.Category)
               .Include(ur => ur.Executor)
               .Include(ur => ur.Lifecycle)
               .Where(ur => ur.Status == 1)
               .ToList();

            var _requests = _db.Requests
               .Include(ur => ur.Lifecycle)
               .ToList();

            int plannedRequestsCount = _requests.Count(r => r.Status == 1);
            int openedRequestsCount = _requests.Count(r => r.Status == 2);
            int closedRequestsCount = _requests.Count(r => r.Status == 3);
            int cancelledRequestsCount = _requests.Count(r => r.Status == 4);

            ViewBag.PlanningRequestsCount = plannedRequestsCount;
            ViewBag.CurrentRequestsCount = openedRequestsCount;
            ViewBag.ClosedRequestsCount = closedRequestsCount;
            ViewBag.CancelledRequestsCount = cancelledRequestsCount;

            return View(requests);
        }
        [HttpGet]
        public IActionResult CurrentRequests()
        {
            var requests = _db.Requests
               .Include(ur => ur.Category)
               .Include(ur => ur.Executor)
               .Include(ur => ur.Lifecycle)
               .Include(ur => ur.Opened)
               .Where(ur => ur.Status == 2)
               .ToList();

            var _requests = _db.Requests
               .Include(ur => ur.Lifecycle)
               .ToList();

            int plannedRequestsCount = _requests.Count(r => r.Status == 1);
            int openedRequestsCount = _requests.Count(r => r.Status == 2);
            int closedRequestsCount = _requests.Count(r => r.Status == 3);
            int cancelledRequestsCount = _requests.Count(r => r.Status == 4);

            ViewBag.PlanningRequestsCount = plannedRequestsCount;
            ViewBag.CurrentRequestsCount = openedRequestsCount;
            ViewBag.ClosedRequestsCount = closedRequestsCount;
            ViewBag.CancelledRequestsCount = cancelledRequestsCount;

            return View(requests);
        }
        [HttpGet]
        public IActionResult ClosedRequests()
        {
            var requests = _db.Requests
               .Include(ur => ur.Category)
               .Include(ur => ur.Executor)
               .Include(ur => ur.Lifecycle)
               .Include(ur => ur.Opened)
               .Include(ur => ur.Closed)
               .Where(ur => ur.Status == 3)
               .ToList();

            var _requests = _db.Requests
               .Include(ur => ur.Lifecycle)
               .ToList();

            int plannedRequestsCount = _requests.Count(r => r.Status == 1);
            int openedRequestsCount = _requests.Count(r => r.Status == 2);
            int closedRequestsCount = _requests.Count(r => r.Status == 3);
            int cancelledRequestsCount = _requests.Count(r => r.Status == 4);

            ViewBag.PlanningRequestsCount = plannedRequestsCount;
            ViewBag.CurrentRequestsCount = openedRequestsCount;
            ViewBag.ClosedRequestsCount = closedRequestsCount;
            ViewBag.CancelledRequestsCount = cancelledRequestsCount;

            return View(requests);
        }
        [HttpGet]
        public IActionResult CancelledRequests()
        {
            var requests = _db.Requests
               .Include(ur => ur.Category)
               .Include(ur => ur.Executor)
               .Include(ur => ur.Lifecycle)
               .Include(ur => ur.Opened)
               .Include(ur => ur.Cancelled)
               .Where(ur => ur.Status == 4)
               .ToList();

            var _requests = _db.Requests
               .Include(ur => ur.Lifecycle)
               .ToList();

            int plannedRequestsCount = _requests.Count(r => r.Status == 1);
            int openedRequestsCount = _requests.Count(r => r.Status == 2);
            int closedRequestsCount = _requests.Count(r => r.Status == 3);
            int cancelledRequestsCount = _requests.Count(r => r.Status == 4);

            ViewBag.PlanningRequestsCount = plannedRequestsCount;
            ViewBag.CurrentRequestsCount = openedRequestsCount;
            ViewBag.ClosedRequestsCount = closedRequestsCount;
            ViewBag.CancelledRequestsCount = cancelledRequestsCount;

            return View(requests);
        }
        [HttpGet]
        public IActionResult Open(uint id)
        {
            var request = _db.Requests.Include(r => r.Lifecycle).FirstOrDefault(r => r.Id == id);
            if (request != null)
            {
                request.OpenId = Convert.ToUInt32(Request.Cookies["Id"]);
                request.Lifecycle.Current = DateTime.UtcNow;
                request.Status = 2;
                _db.SaveChanges();
            }
            return RedirectToAction("CurrentRequests");
        }
        [HttpGet]
        public IActionResult Close(uint id)
        {
            var request = _db.Requests.Include(r => r.Lifecycle).FirstOrDefault(r => r.Id == id);
            if (request != null)
            {
                request.CloseId = Convert.ToUInt32(Request.Cookies["Id"]);
                request.Lifecycle.Closed = DateTime.UtcNow;
                request.Status = 3;
                _db.SaveChanges();
            }
            return RedirectToAction("ClosedRequests");
        }
        [HttpGet]
        public IActionResult Cancel(uint id)
        {
            var request = _db.Requests.Include(r => r.Lifecycle).FirstOrDefault(r => r.Id == id);
            if (request != null)
            {
                request.CancelId = Convert.ToUInt32(Request.Cookies["Id"]);
                request.Lifecycle.Cancelled = DateTime.UtcNow;
                request.Status = 4;
                _db.SaveChanges();
            }
            return RedirectToAction("CancelledRequests");
        }
    }
}
