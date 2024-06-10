using Business.Abstract;
using Business.Constans;
using Core.Entities.Concrete;
using Core.Utilities.Results.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("createuser")]
		public async Task<IActionResult> CreateUser(UserDto userDto)
		{
			var createResult = await _userService.AddAsync(userDto);
			if (!createResult.Success)
				return BadRequest(createResult.Message);

			return Ok(createResult.Message);
		}

		[HttpPost("updatebyuserdto")]
		public async Task<IActionResult> UpdateByUserDtoAsync(UserDto userDto)
		{
			var updateResult = await _userService.UpdateByUserDtoAsync(userDto);
			if (!updateResult.Success)
				return BadRequest(updateResult.Message);

			return Ok(updateResult.Message);
		}

		[HttpDelete("deleteuser")]
		public async Task<IActionResult> DeleteUser(int userId)
		{
			var deleteResult = await _userService.DeleteAsync(userId);
			if (!deleteResult.Success)
				return BadRequest(deleteResult.Message);

			return Ok(deleteResult.Message);
		}

		[HttpGet("getuser")]
		public IActionResult GetUser(int userId)
		{
			var userResult = _userService.GetById(userId);
			if (!userResult.Success)
				return BadRequest(userResult.Message);

			return Ok(userResult.Data);
		}
		
		[HttpGet("getuserlist")]
		public async Task<IActionResult> GetUserList()
		{
			var userListResult = await _userService.GetAllAsync();
			if (!userListResult.Success)
				return BadRequest(userListResult.Message);

			return Ok(userListResult.Data);
		}
		
	
	}
}
