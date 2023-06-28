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
        public IActionResult Requests()
        {
            
            var requests = _db.Requests
               .Include(ur => ur.Category)
               .Include(ur => ur.Executor)
               .Include(ur => ur.Lifecycle)
               .Include(ur => ur.User)
               .ToList();

            int openedRequestsCount = requests.Count(r => r.Status == 1);
            int processingRequestsCount = requests.Count(r => r.Status == 2);
            int closedRequestsCount = requests.Count(r => r.Status == 3);
            int cancelledRequestsCount = requests.Count(r => r.Status == 4);

            ViewBag.OpenedRequestsCount = openedRequestsCount;
            ViewBag.ProcessingRequestsCount = processingRequestsCount;
            ViewBag.ClosedRequestsCount = closedRequestsCount;
            ViewBag.CancelledRequestsCount = cancelledRequestsCount;

            return View(requests);
        }
        [HttpGet]
        public IActionResult Close(uint id)
        {
            var request = _db.Requests.Include(r => r.Lifecycle).FirstOrDefault(r => r.Id == id);
            if (request != null)
            {
                request.UserId = Convert.ToUInt32(Request.Cookies["Id"]);
                request.Lifecycle.Closed = DateTime.UtcNow;
                request.Status = 3;
                _db.SaveChanges();
            }
            return RedirectToAction("Requests");
        }
        [HttpGet]
        public IActionResult Cancel(uint id)
        {
            var request = _db.Requests.Include(r => r.Lifecycle).FirstOrDefault(r => r.Id == id);
            if (request != null)
            {
                request.UserId = Convert.ToUInt32(Request.Cookies["Id"]);
                request.Lifecycle.Cancelled = DateTime.UtcNow;
                request.Status = 4;
                _db.SaveChanges();
            }
            return RedirectToAction("Requests");
        }
    }
}
