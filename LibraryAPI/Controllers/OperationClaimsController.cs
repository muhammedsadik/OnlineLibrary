using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OperationClaimsController : ControllerBase
	{
		private IOperationClaimService _operationClaimService;

		public OperationClaimsController(IOperationClaimService operationClaimService)
		{
			_operationClaimService = operationClaimService;
		}

		[HttpPost("createoperationClaim")]
		public async Task<IActionResult> CreateOperationClaim(OperationClaimDto operationClaimDto)
		{
			var createResult = await _operationClaimService.AddAsync(operationClaimDto);
			if (!createResult.Success)
				return BadRequest(createResult.Message);

			return Ok(createResult.Message);
		}

		[HttpPost("updateoperationClaim")]
		public async Task<IActionResult> UpdateOperationClaim(OperationClaimDto operationClaimDto)
		{
			var updateResult = await _operationClaimService.UpdateAsync(operationClaimDto);
			if (!updateResult.Success)
				return BadRequest(updateResult.Message);

			return Ok(updateResult.Message);
		}

		[HttpDelete("deleteoperationClaim")]
		public async Task<IActionResult> DeleteOperationClaim(int operationClaimId)
		{
			var deleteResult = await _operationClaimService.DeleteAsync(operationClaimId);
			if (!deleteResult.Success)
				return BadRequest(deleteResult.Message);

			return Ok(deleteResult.Message);
		}

		[HttpGet("getoperationclaimbyid")]
		public IActionResult GetOperationClaimById(int operationClaimId)
		{
			var operationClaimResult = _operationClaimService.GetById(operationClaimId);
			if (!operationClaimResult.Success)
				return BadRequest(operationClaimResult.Message);

			return Ok(operationClaimResult.Data);
		}

		[HttpGet("getoperationClaimlist")]
		public async Task<IActionResult> GetOperationClaimList()
		{
			var operationClaimResult = await _operationClaimService.GetAllAsync();
			if (!operationClaimResult.Success)
				return BadRequest(operationClaimResult.Message);

			return Ok(operationClaimResult.Data);
		}

	}
}
