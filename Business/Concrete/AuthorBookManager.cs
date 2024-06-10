using Business.Abstract;
using Business.Constans;
using Core.Utilities.Business;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class AuthorBookManager : IAuthorBookService
	{
		private readonly IAuthorBookDal _authorBookDal;
		private readonly IAuthorService _authorService;
		private readonly IBookService _bookService;

		public AuthorBookManager(IAuthorBookDal authorBookDal, IAuthorService authorService, IBookService bookService)
		{
			_authorBookDal = authorBookDal;
			_authorService = authorService;
			_bookService = bookService;
		}

		public async Task<IResult> AddAsync(AuthorBooksDto authorBooksDto)
		{
			var authorExist = GetAuthorByAuthorId(authorBooksDto.AuthorId);
			if (!authorExist.Success) return new ErrorResult(authorExist.Message);

			foreach (var bookId in authorBooksDto.BookId)
			{
				var result = GetBookByBookId(bookId);
				if (!result.Success)
				{
					return new ErrorResult($"The book id : {bookId} is not found");
				}

				var authorBookExist = _authorBookDal.Get(x => x.AuthorId == authorBooksDto.AuthorId && x.BookId == bookId);
				if (authorBookExist != null)
				{
					return new ErrorResult($"The Author and Book id : {authorBookExist.Id} is already exist");
				}

				AuthorBook authorBook = new();
				authorBook.BookId = bookId;
				authorBook.AuthorId = authorBooksDto.AuthorId;
				authorBook.AddedAt = DateTime.Now;

				await _authorBookDal.AddAsync(authorBook);
			}

			return new SuccessResult(Messages.AuthorBookAdded);
		}

		public async Task<IResult> DeleteAsync(AuthorBooksDto authorBooksDto)
		{
			var isAuthorBookExist = GetListByBooksIdAndAuthorId(authorBooksDto);
			if (!isAuthorBookExist.Success) return await Task.FromResult<IResult>(new ErrorResult(isAuthorBookExist.Message));

			foreach (var authorBook in isAuthorBookExist.Data)
			{
				await _authorBookDal.DeleteAsync(authorBook);
			}

			return new SuccessResult(Messages.AuthorBookDeleted);
		}

		public async Task<IDataResult<List<AuthorBook>>> GetAllAsync()
		{
			var authorBookList = await _authorBookDal.GetAllAsync();
			if (authorBookList == null) return new ErrorDataResult<List<AuthorBook>>(Messages.AuthorBookNotFound);

			return new SuccessDataResult<List<AuthorBook>>(authorBookList);
		}

		public IDataResult<List<AuthorBook>> GetAuthorBookListByAuthorId(int authorId)
		{
			var authorExist = GetAuthorByAuthorId(authorId);
			if (!authorExist.Success) return new ErrorDataResult<List<AuthorBook>>(authorExist.Message);

			var bookList = _authorBookDal.Where(x => x.AuthorId == authorId).ToList();
			if (bookList.Count == 0) return new ErrorDataResult<List<AuthorBook>>(Messages.BookNotFound);

			return new SuccessDataResult<List<AuthorBook>>(bookList);
		}

		public IDataResult<AuthorBook> GetByAuthorBookId(int id)
		{
			var isAuthorBookExist = _authorBookDal.Get(x => x.Id == id);
			if (isAuthorBookExist == null) return new ErrorDataResult<AuthorBook>(Messages.AuthorBookNotFound);

			return new SuccessDataResult<AuthorBook>(isAuthorBookExist);
		}

		public IDataResult<List<AuthorBook>> GetListByBooksIdAndAuthorId(AuthorBooksDto authorBooksDto)
		{
			var authorExist = GetAuthorByAuthorId(authorBooksDto.AuthorId);
			if (!authorExist.Success) return new ErrorDataResult<List<AuthorBook>>(authorExist.Message);

			var authorBooks = new List<AuthorBook>();
			foreach (var bookId in authorBooksDto.BookId)
			{
				var result = GetBookByBookId(bookId);
				if (!result.Success)
				{
					return new ErrorDataResult<List<AuthorBook>>($"The book id : {bookId} is not found");
				}

				var authorBookExist = _authorBookDal.Get(x => x.AuthorId == authorBooksDto.AuthorId && x.BookId == bookId);
				if (authorBookExist == null)
				{
					return new ErrorDataResult<List<AuthorBook>>($"The bookid : {bookId} , author id : {authorBooksDto.AuthorId} is not found");
				}

				authorBooks.Add(authorBookExist);
			}

			return new SuccessDataResult<List<AuthorBook>>(authorBooks);
		}

		public async Task<IResult> UpdateAsync(AuthorBookUpdateDto authorBooksUpdateDto)
		{
			var isSame = _authorBookDal.IsExist(x => x.AuthorId == authorBooksUpdateDto.AuthorId && x.BookId == authorBooksUpdateDto.BookId);
			if (isSame == true) return await Task.FromResult<IResult>(new ErrorResult(Messages.AuthorBookAlreadyExists));


			var isAuthorBookExist = GetByAuthorBookId(authorBooksUpdateDto.Id);
			if (!isAuthorBookExist.Success) return await Task.FromResult<IResult>(new ErrorResult(isAuthorBookExist.Message));

			/*Yarın bunları paylaşacaksın

			var authorExist = GetAuthorByAuthorId(authorBooksUpdateDto.AuthorId);
			if (!authorExist.Success) return await Task.FromResult<IResult>(new ErrorResult(authorExist.Message));

			var bookExist = GetBookByBookId(authorBooksUpdateDto.BookId);
			if (!bookExist.Success) return await Task.FromResult<IResult>(new ErrorResult(bookExist.Message));
			*/

			IResult result = BusinessRoles.Run(GetAuthorByAuthorId(authorBooksUpdateDto.AuthorId), GetBookByBookId(authorBooksUpdateDto.BookId));
			if (result != null)
			{
				return result;
			}

			isAuthorBookExist.Data.AuthorId = authorBooksUpdateDto.AuthorId;
			isAuthorBookExist.Data.BookId = authorBooksUpdateDto.BookId;
			isAuthorBookExist.Data.AddedAt = DateTime.Now;

			await _authorBookDal.UpdateAsync(isAuthorBookExist.Data);

			return new SuccessResult(Messages.AuthorBookUpdated);
		}


		public IDataResult<List<Author>> GetAuthorByBookId(int bookId)
		{
			var bookExist = GetBookByBookId(bookId);
			if (!bookExist.Success) return new ErrorDataResult<List<Author>>(bookExist.Message);

			var authorBook = GetAuthorBookListByBookId(bookId);
			if (!authorBook.Success) return new ErrorDataResult<List<Author>>(authorBook.Message);


			List<Author> authorList = new();

			foreach (var item in authorBook.Data)
			{
				var author = GetAuthorByAuthorId(item.AuthorId).Data;
				if (author == null) return new ErrorDataResult<List<Author>>(Messages.AuthorNotFound);

				authorList.Add(author);
			}


			return new SuccessDataResult<List<Author>>(authorList);
		}

		public IDataResult<List<AuthorBook>> GetAuthorBookListByBookId(int bookId)
		{
			var authorBook = _authorBookDal.Where(x => x.BookId == bookId);
			if (authorBook == null) return new ErrorDataResult<List<AuthorBook>>(Messages.AuthorBookNotFound);

			return new SuccessDataResult<List<AuthorBook>>(authorBook.ToList());
		}

		public IDataResult<List<Book>> GetBookListByAuthorId(int authorId)
		{
			var authorExist = GetAuthorByAuthorId(authorId);
			if (!authorExist.Success) return new ErrorDataResult<List<Book>>(authorExist.Message);

			var authorBookExist = GetAuthorBookListByAuthorId(authorId);
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


		private IDataResult<Author> GetAuthorByAuthorId(int authorId)
		{
			var authorExist = _authorService.GetById(authorId);
			if (!authorExist.Success) return new ErrorDataResult<Author>(authorExist.Message);

			return new SuccessDataResult<Author>(authorExist.Data);
		}


		private IDataResult<Book> GetBookByBookId(int bookId)
		{
			var bookExist = _bookService.GetById(bookId);
			if (!bookExist.Success) return new ErrorDataResult<Book>(bookExist.Message);

			return new SuccessDataResult<Book>(bookExist.Data);
		}


	}
}
