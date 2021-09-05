using Application.Session;
using Application.Session.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IntechWebApp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SessionController : ControllerBase
	{
		private readonly ILogger<SessionController> _logger;
		private readonly ISessionService _sessionService;

		public SessionController(ILogger<SessionController> logger, ISessionService sessionService)
		{
			_logger = logger;
			_sessionService = sessionService;
		}

		[HttpPost("Create")]
		public async Task<IActionResult> Create(SessionModel model)
		{
			try
			{
				var session = await _sessionService.CreateAsync(model);

				return new JsonResult(session);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);

				return new BadRequestObjectResult(new 
				{
					HasError = true,
					Message = ex.Message
				});
			}
		}
	}
}
