using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserOperationClaimsController : ControllerBase
	{
		private readonly IUserOperationClaimService _userOperationClaimService;

		public UserOperationClaimsController(IUserOperationClaimService userOperationClaimService)
		{
			_userOperationClaimService = userOperationClaimService;
		}

		[HttpPost("creatuseroperationclaim")]
		public async Task<IActionResult> CreateBookCategories(UserOperationClaimDto userOperationClaimDto)
		{
			var createResult = await _userOperationClaimService.AddAsync(userOperationClaimDto);
			if (!createResult.Success)
				return BadRequest(createResult.Message);

			return Ok(createResult.Message);
		}

		[HttpPost("updateuseroperationclaim")]
		public async Task<IActionResult> UpdateBookCategories(UserOperationClaimUpdateDto userOperationClaimUpdateDto)
		{
			var updateResult = await _userOperationClaimService.UpdateAsync(userOperationClaimUpdateDto);
			if (!updateResult.Success)
				return BadRequest(updateResult.Message);

			return Ok(updateResult.Message);
		}

		[HttpPost("deleteuseroperationclaim")]
		public async Task<IActionResult> DeleteBookCategories(UserOperationClaimDto userOperationClaimDto)
		{
			var deleteResult = await _userOperationClaimService.DeleteAsync(userOperationClaimDto);
			if (!deleteResult.Success)
				return BadRequest(deleteResult.Message);

			return Ok(deleteResult.Message);
		}

		[HttpGet("getbyuseroperationclaimid")]
		public IActionResult GetByUserOperationClaimId(int id)
		{
			var authorBookResult = _userOperationClaimService.GetByUserOperationClaimId(id);
			if (!authorBookResult.Success)
				return BadRequest(authorBookResult.Message);

			return Ok(authorBookResult.Data);
		}

		[HttpGet("getuseroperationclaimlist")]
		public async Task<IActionResult> GetUserOperationClaimList()
		{
			var bookCategoryListResult = await _userOperationClaimService.GetAllAsync();
			if (!bookCategoryListResult.Success)
				return BadRequest(bookCategoryListResult.Message);

			return Ok(bookCategoryListResult.Data);
		}
		
		[HttpPost("getlistbyspecificid")]
		public IActionResult GetListBySpecificId(UserOperationClaimDto userOperationClaimDto)
		{
			var bookCategoryListResult = _userOperationClaimService.GetListBySpecificId(userOperationClaimDto);
			if (!bookCategoryListResult.Success)
				return BadRequest(bookCategoryListResult.Message);

			return Ok(bookCategoryListResult.Data);
		}

		[HttpGet("getuseroperationclaimlistbyuserid")]
		public IActionResult GetUserOperationClaimListByUserId(int userId)
		{
			var userOperationClaimListResult = _userOperationClaimService.GetUserOperationClaimListByuserId(userId);
			if (!userOperationClaimListResult.Success)
				return BadRequest(userOperationClaimListResult.Message);
			
			return Ok(userOperationClaimListResult.Data);
		}

		[HttpGet("getoperationclaimlistbyuserid")]
		public IActionResult GetOperationClaimListByUserId(int userId)
		{
			var userOperationClaimListResult = _userOperationClaimService.GetOperationClaimListByUserId(userId);
			if (!userOperationClaimListResult.Success)
				return BadRequest(userOperationClaimListResult.Message);

			return Ok(userOperationClaimListResult.Data);
		}

		[HttpGet("getuserlistbyoperationclaimid")]
		public IActionResult GetUserListByOperationClaimId(int operationClaimId)
		{
			var userListResult = _userOperationClaimService.GetUserListByOperationClaimId(operationClaimId);
			if (!userListResult.Success)
				return BadRequest(userListResult.Message);

			return Ok(userListResult.Data);
		}

		[HttpGet("getuseroperationclaimlistbyoperationclaimid")]
		public IActionResult GetUserOperationClaimListByoperationClaimId(int operationClaimId)
		{
			var userListResult = _userOperationClaimService.GetUserOperationClaimListByoperationClaimId(operationClaimId);
			if (!userListResult.Success)
				return BadRequest(userListResult.Message);

			return Ok(userListResult.Data);
		}


		//CompanyId => List<UserOperationClaim>
		//CompanyId => List<UserAndOperationClaim>



	}
}
