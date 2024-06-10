using Business.Abstract;
using Business.Constans;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class CategoryBookManager : ICategoryBookService
	{
		private readonly ICategoryBookDal _bookCategoryDal;
		private readonly ICategoryService _categoryService;
		private readonly IBookService _bookService;
		 
		public CategoryBookManager(ICategoryBookDal bookCategoryDal, ICategoryService categoryService, IBookService bookService)
		{
			_bookCategoryDal = bookCategoryDal;
			_categoryService = categoryService;
			_bookService = bookService;
		}

		public async Task<IResult> AddAsync(BooksCategoryDto booksCategoryDto)
		{
			var categoryExist = GetCategoryByCategoryId(booksCategoryDto.CategoryId);
			if (!categoryExist.Success) return new ErrorResult(categoryExist.Message);

			foreach (var bookId in booksCategoryDto.BookId)
			{
				var result = GetBookByBookId(bookId);
				if (!result.Success)
				{
					return new ErrorResult($"The book id : {bookId} is not found");
				}

				var categoryBookExist = _bookCategoryDal.Get(x => x.BookId == bookId && x.CategoryId == booksCategoryDto.CategoryId);
				if (categoryBookExist != null)
				{
					return new ErrorResult($"The Category and Book id : {categoryBookExist.Id} is already exist");
				}

				BookCategory bookCategory = new();
				bookCategory.CategoryId = booksCategoryDto.CategoryId;
				bookCategory.BookId = bookId;
				bookCategory.AddedAt = DateTime.Now;

				await _bookCategoryDal.AddAsync(bookCategory);
			}

			return new SuccessResult(Messages.BookCategoryAdded);
		}

		public async Task<IResult> DeleteAsync(BooksCategoryDto booksCategoryDto)
		{
			var isBookCategoryExist = GetListByBooksIdAndCategoryId(booksCategoryDto);
			if (!isBookCategoryExist.Success) return await Task.FromResult<IResult>(new ErrorResult(isBookCategoryExist.Message));

			foreach (var bookCategory in isBookCategoryExist.Data)
			{
				await _bookCategoryDal.DeleteAsync(bookCategory);
			}

			return new SuccessResult(Messages.BookCategoryDeleted);
		}

		public async Task<IDataResult<List<BookCategory>>> GetAllAsync()
		{
			var bookCategoryList = await _bookCategoryDal.GetAllAsync();
			if (bookCategoryList == null) return new ErrorDataResult<List<BookCategory>>(Messages.BookCategoryNotFound);

			return new SuccessDataResult<List<BookCategory>>(bookCategoryList);
		}

		public IDataResult<List<BookCategory>> GetBookCategoryListByCategoryId(int categoryId)
		{
			var categoryExist = GetCategoryByCategoryId(categoryId);
			if (!categoryExist.Success) return new ErrorDataResult<List<BookCategory>>(categoryExist.Message);

			var bookCategoryList = _bookCategoryDal.Where(x => x.CategoryId == categoryId).ToList();
			if (bookCategoryList.IsNullOrEmpty()) return new ErrorDataResult<List<BookCategory>>(Messages.BookCategoryListNotFound);
			//hata alırsan IsNullOrEmpty yi Count == 0 ile değştr 

			return new SuccessDataResult<List<BookCategory>>(bookCategoryList);
		}

		public IDataResult<BookCategory> GetBookCategoryByBookCategoryId(int id)
		{
			var isBookCategoryExist = _bookCategoryDal.Get(x => x.Id == id);
			if (isBookCategoryExist == null) return new ErrorDataResult<BookCategory>(Messages.BookCategoryNotFound);

			return new SuccessDataResult<BookCategory>(isBookCategoryExist);
		}

		public IDataResult<List<BookCategory>> GetListByBooksIdAndCategoryId(BooksCategoryDto booksCategoryDto)
		{
			var categoryExist = GetCategoryByCategoryId(booksCategoryDto.CategoryId);
			if (!categoryExist.Success) return new ErrorDataResult<List<BookCategory>>(categoryExist.Message);

			var bookcategories = new List<BookCategory>();
			foreach (var bookId in booksCategoryDto.BookId)
			{
				var result = GetBookByBookId(bookId);
				if (!result.Success)
				{
					return new ErrorDataResult<List<BookCategory>>($"The book id : {bookId} is not found");
				}

				var bookCategoryExist = _bookCategoryDal.Get(x => x.BookId == bookId && x.CategoryId == booksCategoryDto.CategoryId);
				if (bookCategoryExist == null)
				{
					return new ErrorDataResult<List<BookCategory>>($"The book id : {bookId} , category id : {booksCategoryDto.CategoryId} is not found");
				}

				bookcategories.Add(bookCategoryExist);
			}

			return new SuccessDataResult<List<BookCategory>>(bookcategories);
		}

		public async Task<IResult> UpdateAsync(BookCategoryUpdateDto bookCategoryUpdateDto)
		{
			var isSame = _bookCategoryDal.IsExist(x => x.CategoryId == bookCategoryUpdateDto.CategoryId && x.BookId == bookCategoryUpdateDto.BookId);
			if (isSame == true) return await Task.FromResult<IResult>(new ErrorResult(Messages.BookCategoryAlreadyExists));


			var isBookCategoryExist = _bookCategoryDal.Get(x => x.Id == bookCategoryUpdateDto.Id);
			if (isBookCategoryExist == null) return await Task.FromResult<IResult>(new ErrorResult(Messages.BookCategoryNotFound));

			var categoryExist = GetCategoryByCategoryId(bookCategoryUpdateDto.CategoryId);
			if (!categoryExist.Success) return await Task.FromResult<IResult>(new ErrorResult(categoryExist.Message));

			var bookResult = GetBookByBookId(bookCategoryUpdateDto.BookId);
			if (!bookResult.Success) return await Task.FromResult<IResult>(new ErrorResult(bookResult.Message));

			isBookCategoryExist.CategoryId = bookCategoryUpdateDto.CategoryId;
			isBookCategoryExist.BookId = bookCategoryUpdateDto.BookId;
			isBookCategoryExist.AddedAt = DateTime.Now;

			await _bookCategoryDal.UpdateAsync(isBookCategoryExist);

			return new SuccessResult(Messages.BookCategoryUpdated);
		}

		public IDataResult<BookCategory> GetBookCategoryByBookId(int bookId)
		{
			var bookCategory = _bookCategoryDal.Get(x => x.BookId == bookId);
			if (bookCategory == null) return new ErrorDataResult<BookCategory>(Messages.BookCategoryNotFound);

			return new SuccessDataResult<BookCategory>(bookCategory);
		}
		
		public IDataResult<List<Category>> GetCategoryByBookId(int bookId)
		{
			var bookExist = GetBookByBookId(bookId);
			if (!bookExist.Success) return new ErrorDataResult<List<Category>>(bookExist.Message);

			var bookCategory = GetBookCategoryListByBookId(bookId);
			if (!bookCategory.Success) return new ErrorDataResult<List<Category>>(bookCategory.Message);
			

			List<Category> categoryList = new();

			foreach (var item in bookCategory.Data)
			{
				var author = GetCategoryByCategoryId(item.CategoryId).Data;
				if (author == null) return new ErrorDataResult<List<Category>>(Messages.AuthorNotFound);

				categoryList.Add(author);
			}

			return new SuccessDataResult<List<Category>>(categoryList);
		}

		private IDataResult<List<BookCategory>> GetBookCategoryListByBookId(int bookId)
		{
			var bookCategory = _bookCategoryDal.Where(x => x.BookId == bookId);
			if (bookCategory == null) return new ErrorDataResult<List<BookCategory>>(Messages.AuthorBookNotFound);

			return new SuccessDataResult<List<BookCategory>>(bookCategory.ToList());
		}

		public IDataResult<List<Book>> GetBookListByCategoryId(int categoryId)
		{
			var authorExist = GetCategoryByBookId(categoryId);
			if (!authorExist.Success) return new ErrorDataResult<List<Book>>(authorExist.Message);

			var authorBookExist = GetBookCategoryListByCategoryId(categoryId);
			if (!authorBookExist.Success) return new ErrorDataResult<List<Book>>(authorBookExist.Message);

			var bookList = new List<Book>();
			foreach (var authorBook in authorBookExist.Data)
			{
				var book = GetBookByBookId(authorBook.BookId);
				if (!book.Success) return new ErrorDataResult<List<Book>>(book.Message);

				bookList.Add(book.Data);
			}

			return new SuccessDataResult<List<Book>>(bookList);
		}

		private IDataResult<Category> GetCategoryByCategoryId(int categoryId)
		{
			var categoryExist = _categoryService.GetById(categoryId);
			if (!categoryExist.Success) return new ErrorDataResult<Category>(categoryExist.Message);

			return new SuccessDataResult<Category>(categoryExist.Data);
		}


		private IDataResult<Book> GetBookByBookId(int bookId)
		{
			var bookExist = _bookService.GetById(bookId);
			if (!bookExist.Success) return new ErrorDataResult<Book>(bookExist.Message);

			return new SuccessDataResult<Book>(bookExist.Data);
		}

	}
}
