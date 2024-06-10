using Business.Abstract;
using Business.BusinessAspect;
using Business.Constans;
using Business.ValidationRules.FluentValidation;
using Core.Aspect.Autofac.Caching;
using Core.Aspect.Autofac.Validation;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class BookManager : IBookService
	{
		private readonly IBookDal _bookDal;

		public BookManager(IBookDal boolDal)
		{
			_bookDal = boolDal;
		}


		[SecuredOperation("Book.Create")]
		[CacheRemoveAspect(pattern: "IBookService.Get")]
		[ValidationAspect(typeof(BookValidator))]
		public async Task<IResult> AddAsync(BookDto bookDto)
		{
			var bookExist = _bookDal.IsExist(b => b.Name == bookDto.Name);
			if (bookExist) return await Task.FromResult<IResult>(new ErrorResult(Messages.BookAlreadyExists));

			var book = new Book
			{
				Name = bookDto.Name,
				Language = bookDto.Language,
				Price = bookDto.Price,
				Description = bookDto.Description,
				AddedAt = DateTime.Now,
			};
			await _bookDal.AddAsync(book);

			return new SuccessResult(Messages.BookAdded);
		}


		[SecuredOperation("Book.Delete")]
		[CacheRemoveAspect(pattern: "IBookService.Get")]
		public async Task<IResult> DeleteAsync(int bookId)
		{
			var bookExist = GetById(bookId);
			if (!bookExist.Success) return new ErrorResult(bookExist.Message);

			await _bookDal.DeleteAsync(bookExist.Data);

			return new SuccessResult(Messages.BookDeleted);
		}


		[SecuredOperation("Book.List")]
		[CacheAspect(duration: 10)]
		public async Task<IDataResult<List<Book>>> GetAllAsync()
		{
			var bookList = await _bookDal.GetAllAsync();
			if (bookList == null) return new ErrorDataResult<List<Book>>(Messages.BookNotFound);

			return new SuccessDataResult<List<Book>>(bookList);
		}


		[SecuredOperation("Book.List")]
		public IDataResult<Book> GetById(int id)
		{
			var result = _bookDal.Get(a => a.Id == id);
			if (result == null)
				return new ErrorDataResult<Book>(Messages.BookNotFound);

			return new SuccessDataResult<Book>(result);
		}


		[SecuredOperation("Book.Update")]
		[CacheRemoveAspect(pattern: "IBookService.Get")]
		[ValidationAspect(typeof(BookValidator))]
		public async Task<IResult> UpdateAsync(BookDto bookDto)
		{
			var bookExist = _bookDal.IsExist(b => b.Name == bookDto.Name);
			if (bookExist) return await Task.FromResult<IResult>(new ErrorResult(Messages.BookAlreadyExists));

			var book = GetById(bookDto.Id).Data;
			if (book == null) return await Task.FromResult<IResult>(new ErrorResult(Messages.BookNotFound));

			book.Name = bookDto.Name;
			book.Description = bookDto.Description;
			book.Language = bookDto.Language;
			book.Price = bookDto.Price;
			book.AddedAt = DateTime.Now;

			await _bookDal.UpdateAsync(book);

			return new SuccessResult(Messages.BookUpdated);
		}


	}
}
