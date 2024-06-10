using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IAuthorBookService
	{
		Task<IResult> AddAsync(AuthorBooksDto authorBooksDto);
		Task<IResult> UpdateAsync(AuthorBookUpdateDto authorBooksUpdateDto);
		Task<IResult> DeleteAsync(AuthorBooksDto authorBooksDto);
		IDataResult<AuthorBook> GetByAuthorBookId(int id);
		Task<IDataResult<List<AuthorBook>>> GetAllAsync();
		IDataResult<List<AuthorBook>> GetListByBooksIdAndAuthorId(AuthorBooksDto authorBooksDto);

		IDataResult<List<AuthorBook>> GetAuthorBookListByAuthorId(int authorId);
		IDataResult<List<AuthorBook>> GetAuthorBookListByBookId(int bookId);
		IDataResult<List<Author>> GetAuthorByBookId(int bookId);
		IDataResult<List<Book>> GetBookListByAuthorId(int authorId);

	}
}
