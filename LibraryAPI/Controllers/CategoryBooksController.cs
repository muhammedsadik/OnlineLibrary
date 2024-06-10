using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryBooksController : ControllerBase
	{
		private ICategoryBookService _bookCategoryService;

		public CategoryBooksController(ICategoryBookService bookCategoryService)
		{
			_bookCategoryService = bookCategoryService;
		}

		[HttpPost("createbookcategories")]
		public async Task<IActionResult> CreateBookCategories(BooksCategoryDto booksCategoryDto)
		{
			var createResult = await _bookCategoryService.AddAsync(booksCategoryDto);
			if (!createResult.Success)
				return BadRequest(createResult.Message);

			return Ok(createResult.Message);
		}

		[HttpPost("updatebookcategories")]
		public async Task<IActionResult> UpdateBookCategories(BookCategoryUpdateDto bookCategoryUpdateDto)
		{
			var updateResult = await _bookCategoryService.UpdateAsync(bookCategoryUpdateDto);
			if (!updateResult.Success)
				return BadRequest(updateResult.Message);

			return Ok(updateResult.Message);
		}

		[HttpPost("deletebookcategories")]
		public async Task<IActionResult> DeleteBookCategories(BooksCategoryDto booksCategoryDto)
		{
			var deleteResult = await _bookCategoryService.DeleteAsync(booksCategoryDto);
			if (!deleteResult.Success)
				return BadRequest(deleteResult.Message);

			return Ok(deleteResult.Message);
		}

		[HttpGet("getbybookcategoryid")]
		public IActionResult GetByBookCategoryId(int id)
		{
			var authorBookResult = _bookCategoryService.GetBookCategoryByBookCategoryId(id);
			if (!authorBookResult.Success)
				return BadRequest(authorBookResult.Message);

			return Ok(authorBookResult.Data);
		}

		[HttpGet("getbookcategorylist")]
		public async Task<IActionResult> GetBookCategoryList()
		{
			var bookCategoryListResult = await _bookCategoryService.GetAllAsync();
			if (!bookCategoryListResult.Success)
				return BadRequest(bookCategoryListResult.Message);

			return Ok(bookCategoryListResult.Data);
		}

		[HttpGet("getbookcategorylistbycategoryid")]
		public IActionResult GetBookCategoryListByCategoryId(int categoryid)
		{
			var bookCategoryListResult = _bookCategoryService.GetBookCategoryListByCategoryId(categoryid);
			if (!bookCategoryListResult.Success)
				return BadRequest(bookCategoryListResult.Message);

			return Ok(bookCategoryListResult.Data);
		}

		[HttpPost("getlistbybooksidandcategoryid")]
		public IActionResult GetListByBooksIdAndAuthorId(BooksCategoryDto booksCategoryDto)
		{
			var bookCategoryListResult = _bookCategoryService.GetListByBooksIdAndCategoryId(booksCategoryDto);
			if (!bookCategoryListResult.Success)
				return BadRequest(bookCategoryListResult.Message);

			return Ok(bookCategoryListResult.Data);
		}



		[HttpGet("getcategorybybookid")]
		public IActionResult GetCategoryByBookId(int bookid)
		{
			var categoryResult = _bookCategoryService.GetCategoryByBookId(bookid);
			if (!categoryResult.Success)
				return BadRequest(categoryResult.Message);

			return Ok(categoryResult.Data);
		}


		[HttpGet("getbooklistbycategoryid")]
		public IActionResult GetBookListByCategoryId(int categoryId)
		{
			var categoryResult = _bookCategoryService.GetBookListByCategoryId(categoryId);
			if (!categoryResult.Success)
				return BadRequest(categoryResult.Message);

			return Ok(categoryResult.Data);
		}
	}
}
