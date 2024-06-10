using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorsController : ControllerBase
	{
		private IAuthorService _authorService;

		public AuthorsController(IAuthorService authorService)
		{
			_authorService = authorService;
		}

		[HttpPost("createauthor")]
		public async Task<IActionResult> CreateAuthor(AuthorDto authorDto)
		{			
			var createResult = await _authorService.AddAsync(authorDto);
			if (!createResult.Success)
				return BadRequest(createResult.Message);

			return Ok(createResult.Message);
		}

		[HttpPost("updateauthor")]
		public async Task<IActionResult> UpdateAuthor(AuthorDto authorDto)
		{
			var updateResult = await _authorService.UpdateAsync(authorDto);
			if (!updateResult.Success)
				return BadRequest(updateResult.Message);

			return Ok(updateResult.Message);
		}

		[HttpDelete("deleteauthor")]
		public async Task<IActionResult> DeleteAuthor(int authorId)
		{
			var deleteResult =await _authorService.DeleteAsync(authorId);
			if (!deleteResult.Success)
				return BadRequest(deleteResult.Message);

			return Ok(deleteResult.Message);
		}

		[HttpGet("getauthorbyid")]
		public IActionResult GetAuthorById(int authorId)
		{
			var authorResult = _authorService.GetById(authorId);
			if (!authorResult.Success)
				return BadRequest(authorResult.Message);

			return Ok(authorResult.Data);
		}

		[HttpGet("getauthorlist")]
		public async Task<IActionResult> GetAuthorList()
		{
			var authorListResult =await _authorService.GetAllAsync();
			if (!authorListResult.Success)
				return BadRequest(authorListResult.Message);

			return Ok(authorListResult.Data);
		}

		



	}
}
