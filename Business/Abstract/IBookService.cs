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
	public interface IBookService
	{
		Task<IResult> AddAsync(BookDto bookDto);
		Task<IResult> UpdateAsync(BookDto bookDto);
		Task<IResult> DeleteAsync(int bookId);
		IDataResult<Book> GetById(int id);
		Task<IDataResult<List<Book>>> GetAllAsync();
	}
}
