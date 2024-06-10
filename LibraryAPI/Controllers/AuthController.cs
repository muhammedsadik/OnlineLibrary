using Business.Abstract;
using Core.Utilities.Results.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(UserDto userDto)
		{
			var registerResult = await _authService.UserRegister(userDto);
			if (!registerResult.Success) return BadRequest(new RegisterDto {
				Data = registerResult.Data,
				Message = registerResult.Message
			});


			return Ok(new RegisterDto {
				Data = registerResult.Data,
				Message = registerResult.Message
			});
		}


		[HttpPost("login")]
		public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
		{
			var loginResult = await _authService.UserLogin(userForLoginDto);
			if (!loginResult.Success) return BadRequest(loginResult.Message);

			return Ok(loginResult.Data);
		}

		[HttpGet("confirmuser")]
		public async Task<IActionResult> ConfirmUser(string value)
		{
			var userExist = _authService.GetByMailConfirmValue(value);
			if (!userExist.Success)
			{
				return BadRequest(userExist.Message);
			}


			userExist.Data.IsMailConfirm = true;
			userExist.Data.MailConfirmDate = DateTime.Now;

			var result =await _authService.UpdateAsync(userExist.Data);
			if (result.Success)
			{
				return Ok(result.Message);
			}

			return BadRequest(result.Message);
		}

	}
}
