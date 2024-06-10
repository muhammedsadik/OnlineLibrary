using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserCompaniesController : ControllerBase
	{
		private readonly IUserCompanyService _userCompanyService;

		public UserCompaniesController(IUserCompanyService userCompanyService)
		{
			_userCompanyService = userCompanyService;
		}

		[HttpPost("createuserscompany")]
		public async Task<IActionResult> CreateUsersCompany(UsersCompanyDto usersCompanyDto)
		{
			var createResult = await _userCompanyService.AddAsync(usersCompanyDto);
			if (!createResult.Success)
				return BadRequest(createResult.Message);

			return Ok(createResult.Message);
		}

		[HttpPost("updateusercompany")]
		public async Task<IActionResult> UpdateUserCompany(UserCompanyUpdateDto userCompanyUpdateDto)
		{
			var updateResult = await _userCompanyService.UpdateAsync(userCompanyUpdateDto);
			if (!updateResult.Success)
				return BadRequest(updateResult.Message);

			return Ok(updateResult.Message);
		}

		[HttpPost("deleteuserscompany")]
		public async Task<IActionResult> DeleteAuthorBooks(UsersCompanyDto usersCompanyDto)
		{
			var deleteResult = await _userCompanyService.DeleteAsync(usersCompanyDto);
			if (!deleteResult.Success)
				return BadRequest(deleteResult.Message);

			return Ok(deleteResult.Message);
		}

		[HttpGet("getbyusercompanyid")]
		public IActionResult GetByUserCompanyId(int id)
		{
			var userCompanyResult = _userCompanyService.GetUserCompanyByUserCompanyId(id);
			if (!userCompanyResult.Success)
				return BadRequest(userCompanyResult.Message);

			return Ok(userCompanyResult.Data);
		}

		[HttpGet("getusercompanylist")]
		public async Task<IActionResult> GetUserCompanyList()
		{
			var userCompanyListResult = await _userCompanyService.GetAllAsync();
			if (!userCompanyListResult.Success)
				return BadRequest(userCompanyListResult.Message);

			return Ok(userCompanyListResult.Data);
		}

		[HttpGet("getusercompanylistbycompanyid")]
		public IActionResult GetUserCompanyListByCompanyId(int companyId)
		{
			var userCompanyListResult = _userCompanyService.GetUserCompanyListByCompanyId(companyId);
			if (!userCompanyListResult.Success)
				return BadRequest(userCompanyListResult.Message);

			return Ok(userCompanyListResult.Data);
		}

		[HttpPost("getlistbyusersidandcompanyid")]
		public IActionResult GetListByUsersIdAndCompanyId(UsersCompanyDto usersCompanyDto)
		{
			var userCompanyListResult = _userCompanyService.GetListByUsersIdAndCompanyId(usersCompanyDto);
			if (!userCompanyListResult.Success)
				return BadRequest(userCompanyListResult.Message);

			return Ok(userCompanyListResult.Data);
		}




		[HttpGet("getcompanybyuserid")]
		public IActionResult GetCompanyByUserId(int userId)
		{
			var companyResult = _userCompanyService.GetCompanyByUserId(userId);
			if (!companyResult.Success)
				return BadRequest(companyResult.Message);

			return Ok(companyResult.Data);
		}

		[HttpGet("getuserlistbycompanyid")]
		public IActionResult GetUserListByCompanyId(int companyId)
		{
			var userListResult = _userCompanyService.GetUserListByCompanyId(companyId);
			if (!userListResult.Success)
				return BadRequest(userListResult.Message);

			return Ok(userListResult.Data);
		}







	}
}
