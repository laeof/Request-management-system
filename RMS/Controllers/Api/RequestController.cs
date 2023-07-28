using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RMS.Attributes;
using RMS.Domain;
using RMS.Domain.Entities;
using System.Runtime.InteropServices;

namespace RMS.Controllers.Api
{
	[ApiController]
	[Route("api/[controller]")]
	[ApiKeyAuthorization]
	public class RequestController : ControllerBase
	{
		private readonly DataManager dataManager;
		private readonly UserManager userManager;

		public RequestController(DataManager dataManager, UserManager userManager)
		{
			this.dataManager = dataManager;
			this.userManager = userManager;
		}

		[HttpGet("all")]
		public async Task<ActionResult<IEnumerable<Request>>> GetRequests(string? apiKey)
		{
			if (apiKey == null)
			{
				return StatusCode(500, new { Message = "Authorize error" });
			}

			await userManager.SignInByApiKeyAsync(apiKey);
			
			var results = dataManager.Requests.GetRequests().ToList();
			return Ok(results);

		}
		[HttpGet("id")]
		public async Task<ActionResult<Request>> GetRequestsById(uint id)
		{
			var results = await dataManager.Requests.GetRequestByIdAsync(id);
			return Ok(results);
		}
	}
}
