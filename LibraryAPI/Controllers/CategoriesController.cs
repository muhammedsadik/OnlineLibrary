using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private ICategoryService _categoryService;

		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpPost("createcategory")]
		public async Task<IActionResult> CreateCategory(CategoryDto categoryDto)
		{
			var createResult = await _categoryService.AddAsync(categoryDto);
			if (!createResult.Success)
				return BadRequest(createResult.Message);

			return Ok(createResult.Message);
		}

		[HttpPost("updatecategory")]
		public async Task<IActionResult> UpdateCategory(CategoryDto categoryDto)
		{
			var updateResult = await _categoryService.UpdateAsync(categoryDto);
			if (!updateResult.Success)
				return BadRequest(updateResult.Message);

			return Ok(updateResult.Message);
		}

		[HttpDelete("deletecategory")]
		public async Task<IActionResult> DeleteCategory(int categoryId)
		{
			var deleteResult = await _categoryService.DeleteAsync(categoryId);
			if (!deleteResult.Success)
				return BadRequest(deleteResult.Message);

			return Ok(deleteResult.Message);
		}

		[HttpGet("getcategorybyid")]
		public IActionResult GetCategoryById(int categoryId)
		{
			var categoryResult = _categoryService.GetById(categoryId);
			if (!categoryResult.Success)
				return BadRequest(categoryResult.Message);

			return Ok(categoryResult.Data);
		}

		[HttpGet("getcategorylist")]
		public async Task<IActionResult> GetCategoryList()
		{
			var categoryListResult = await _categoryService.GetAllAsync();
			if (!categoryListResult.Success)
				return BadRequest(categoryListResult.Message);

			return Ok(categoryListResult.Data);
		}




	}
}
