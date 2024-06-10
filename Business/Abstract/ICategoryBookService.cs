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
	public interface ICategoryBookService
	{
		Task<IResult> AddAsync(BooksCategoryDto booksCategoryDto);
		Task<IResult> UpdateAsync(BookCategoryUpdateDto bookCategoryUpdateDto);
		Task<IResult> DeleteAsync(BooksCategoryDto booksCategoryDto);
		Task<IDataResult<List<BookCategory>>> GetAllAsync();
		IDataResult<BookCategory> GetBookCategoryByBookCategoryId(int id);
		IDataResult<List<BookCategory>> GetListByBooksIdAndCategoryId(BooksCategoryDto booksCategoryDto);
		IDataResult<List<BookCategory>> GetBookCategoryListByCategoryId(int categoryId); 
		IDataResult<BookCategory> GetBookCategoryByBookId(int bookId);
		IDataResult<List<Book>> GetBookListByCategoryId(int categoryId);
		IDataResult<List<Category>> GetCategoryByBookId(int bookId);
	}
}