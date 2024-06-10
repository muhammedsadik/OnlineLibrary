using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorBooksController : ControllerBase
	{
		private readonly IAuthorBookService _authorBookService;

		public AuthorBooksController(IAuthorBookService authorBookService)
		{
			_authorBookService = authorBookService;
		}
		
		[HttpPost("createauthorbooks")]
		public async Task<IActionResult> CreateAuthorBooks(AuthorBooksDto authorBooksDto)
		{
			var createResult = await _authorBookService.AddAsync(authorBooksDto);
			if (!createResult.Success)
				return BadRequest(createResult.Message);

			return Ok(createResult.Message);
		}

		[HttpPost("updateauthorbook")]
		public async Task<IActionResult> UpdateAuthorBooks(AuthorBookUpdateDto authorBooksUpdateDto)
		{
			var updateResult = await _authorBookService.UpdateAsync(authorBooksUpdateDto);
			if (!updateResult.Success)
				return BadRequest(updateResult.Message);

			return Ok(updateResult.Message);
		}

		[HttpPost("deleteauthorbooks")]
		public async Task<IActionResult> DeleteAuthorBooks(AuthorBooksDto authorBooksDto)
		{
			var deleteResult = await _authorBookService.DeleteAsync(authorBooksDto);
			if (!deleteResult.Success)
				return BadRequest(deleteResult.Message);

			return Ok(deleteResult.Message);
		}

		[HttpGet("getbyauthorbookid")]
		public IActionResult GetByAuthorBookId(int id)
		{
			var authorBookResult = _authorBookService.GetByAuthorBookId(id);
			if (!authorBookResult.Success)
				return BadRequest(authorBookResult.Message);

			return Ok(authorBookResult.Data);
		}

		[HttpGet("getauthorbooklist")]
		public async Task<IActionResult> GetAuthorBookList()
		{
			var authorBookListResult = await _authorBookService.GetAllAsync();
			if (!authorBookListResult.Success)
				return BadRequest(authorBookListResult.Message);

			return Ok(authorBookListResult.Data);
		}
				
		[HttpGet("getbookauthorlistbyauthorid")]
		public IActionResult GetBookAuthorListByAuthorId(int authorId)
		{
			var authorBookListResult =  _authorBookService.GetAuthorBookListByAuthorId(authorId);
			if (!authorBookListResult.Success)
				return BadRequest(authorBookListResult.Message);

			return Ok(authorBookListResult.Data);
		}

		[HttpPost("getlistbybookidandauthorid")]
		public IActionResult GetListByBookIdsAndAuthorId(AuthorBooksDto authorBooksDto)
		{
			var authorBookListResult = _authorBookService.GetListByBooksIdAndAuthorId(authorBooksDto);
			if (!authorBookListResult.Success)
				return BadRequest(authorBookListResult.Message);

			return Ok(authorBookListResult.Data);
		}



		[HttpGet("getauthorbybookid")]
		public IActionResult GetAuthorByBookId(int bookid)
		{
			var categoryResult = _authorBookService.GetAuthorByBookId(bookid);
			if (!categoryResult.Success)
				return BadRequest(categoryResult.Message);

			return Ok(categoryResult.Data);
		}
		
		[HttpGet("getbooklistbyauthorid")]
		public IActionResult GetBookListByAuthorId(int authorId)
		{
			var categoryResult = _authorBookService.GetBookListByAuthorId(authorId);
			if (!categoryResult.Success)
				return BadRequest(categoryResult.Message);

			return Ok(categoryResult.Data);
		}



	}
}

