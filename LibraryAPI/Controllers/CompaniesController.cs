using Business.Abstract;
using Core.Utilities.Results.Concrete;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompaniesController : ControllerBase
	{
		private ICompanyService _companyService;
		private readonly ICompanyCommanService _companyCommanService;


		public CompaniesController(ICompanyService companyService, ICompanyCommanService companyCommanService)
		{
			_companyService = companyService;
			_companyCommanService = companyCommanService;
		}

		[HttpPost("createcompany")]
		public async Task<IActionResult> CreateCompany(CompanyDto companyDto)
		{
			var createResult = await _companyService.AddAsync(companyDto);
			if (!createResult.Success)
				return BadRequest(createResult.Message);

			return Ok(createResult.Message);
		}

		[HttpPost("updatecompany")]
		public async Task<IActionResult> UpdateCompany(CompanyDto companyDto)
		{
			var updateResult = await _companyService.UpdateAsync(companyDto);
			if (!updateResult.Success)
				return BadRequest(updateResult.Message);

			return Ok(updateResult.Message);
		}

		[HttpDelete("deletecompany")]
		public async Task<IActionResult> DeleteCompany(int companyId)
		{
			var deleteFromUserOperationClaims = await _companyCommanService.DeleteFromUserOperationClaimTableAsync(companyId);
			if (!deleteFromUserOperationClaims.Success) return BadRequest(deleteFromUserOperationClaims.Message);

			var deleteFromUserCompany = await _companyCommanService.DeleteFromUserCompanyTableAsync(companyId);
			if (!deleteFromUserCompany.Success) return BadRequest(deleteFromUserCompany.Message);

			var deleteResult = await _companyService.DeleteAsync(companyId);
			if (!deleteResult.Success)
				return BadRequest(deleteResult.Message);

			return Ok(deleteResult.Message);
		}
		
		[HttpGet("getcompanybyid")]
		public IActionResult GetCompanyById(int companyId)
		{
			var companyResult = _companyService.GetById(companyId);
			if (!companyResult.Success)
				return BadRequest(companyResult.Message);

			return Ok(companyResult.Data);
		}

		[HttpGet("getcompanylist")]
		public async Task<IActionResult> GetCompanyList()
		{
			var companyListResult = await _companyService.GetAllAsync();
			if (!companyListResult.Success)
				return BadRequest(companyListResult.Message);

			return Ok(companyListResult.Data);
		}




	}
}
