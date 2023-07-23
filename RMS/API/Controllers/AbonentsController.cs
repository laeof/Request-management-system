using Microsoft.AspNetCore.Mvc;
using RMS.Domain.Entities;
using RMS.Service;

namespace RMS.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AbonentsController : ControllerBase
	{
		private readonly Abonents _abonentsService;

		public AbonentsController(Abonents abonentsService)
		{
			_abonentsService = abonentsService;
		}

		[HttpGet("search")]
		public async Task<ActionResult<List<Abonent>>> SearchAbonents(string searchText)
		{
			var results = await _abonentsService.SearchAbonents(searchText);
			return Ok(results);
		}
		[HttpGet("searchuid")]
		public async Task<ActionResult<Abonent>> SearchAbonentsByUID(int uid)
		{
			var results = await _abonentsService.SearchAbonentByUID(uid);
			return Ok(results);
		}
	}
}
