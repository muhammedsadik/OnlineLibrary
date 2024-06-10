using Business.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BooksController : ControllerBase
	{
		private IBookService _bookService;

		public BooksController(IBookService bookService)
		{
			_bookService = bookService;
		}

		[HttpPost("createbook")]
		public async Task<IActionResult> CreateBook(BookDto bookDto)
		{
			var createResult = await _bookService.AddAsync(bookDto);
			if (!createResult.Success)
				return BadRequest(createResult.Message);

			return Ok(createResult.Message);
		}

		[HttpPost("updatebook")]
		public async Task<IActionResult> UpdateBook(BookDto bookDto)
		{
			var updateResult = await _bookService.UpdateAsync(bookDto);
			if (!updateResult.Success)
				return BadRequest(updateResult.Message);

			return Ok(updateResult.Message);
		}

		[HttpDelete("deletebook")]
		public async Task<IActionResult> DeleteBook(int bookId)
		{
			var deleteResult = await _bookService.DeleteAsync(bookId);
			if (!deleteResult.Success)
				return BadRequest(deleteResult.Message);

			return Ok(deleteResult.Message);
		}

		[HttpGet("getbookbyid")]
		public IActionResult GetBookById(int bookId)
		{
			var bookResult = _bookService.GetById(bookId);
			if (!bookResult.Success)
				return BadRequest(bookResult.Message);

			return Ok(bookResult.Data);
		}
		
		[HttpGet("getbooklist")]
		public async Task<IActionResult> GetBookList()
		{
			var bookListResult = await _bookService.GetAllAsync();
			if (!bookListResult.Success)
				return BadRequest(bookListResult.Message);

			return Ok(bookListResult.Data);
		}


		


	}
}
