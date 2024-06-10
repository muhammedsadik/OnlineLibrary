using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MailParametersController : ControllerBase
	{
		private readonly IMailParameterService _mailParameterService;

		public MailParametersController(IMailParameterService mailParameterService)
		{
			_mailParameterService = mailParameterService;
		}

		[HttpPost("update")]
		public async Task<IActionResult> MailParameter(MailParameter mailParameter)
		{
			var result =await _mailParameterService.Update(mailParameter);
			if (result.Success)
			{
				return Ok(result);
			}
			return BadRequest(result.Message);
		}
	}
}
