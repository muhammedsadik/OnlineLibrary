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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class AuthorManager : IAuthorService
	{
		private readonly IAuthorDal _authorDal;
		private readonly IBookService _bookService;

		public AuthorManager(IAuthorDal authorDal, IBookService bookService)
		{
			_authorDal = authorDal;
			_bookService = bookService;
		}


		[CacheRemoveAspect(pattern: "IAuthorService.Get")]
		[SecuredOperation("Author.Create")]
		[ValidationAspect(typeof(AuthorValidator))]
		public async Task<IResult> AddAsync(AuthorDto authorDto)
		{
			var authorExist = _authorDal.IsExist(a => a.FirstName == authorDto.FirstName && a.LastName == authorDto.LastName);
			if (authorExist) return await Task.FromResult<IResult>(new ErrorResult(Messages.AuthorAlreadyExists));

			var author = new Author
			{
				FirstName = authorDto.FirstName,
				LastName = authorDto.LastName,
				Description = authorDto.Description,
				AddedAt = DateTime.Now,
			};

			await _authorDal.AddAsync(author);

			return new SuccessResult(Messages.AuthorAdded);
		}



		[CacheRemoveAspect(pattern: "IAuthorService.Get")]
		[SecuredOperation("Author.Delete")]
		public async Task<IResult> DeleteAsync(int authorId)
		{
			var authorExist = GetById(authorId);
			if (!authorExist.Success) return new ErrorResult(authorExist.Message);

			await _authorDal.DeleteAsync(authorExist.Data);

			return new SuccessResult(Messages.AuthorDeleted);
		}


		[CacheAspect(duration: 10)]
		[SecuredOperation("Author.List")]
		public async Task<IDataResult<List<Author>>> GetAllAsync()
		{
			var authorList = await _authorDal.GetAllAsync();
			if (authorList == null) return new ErrorDataResult<List<Author>>(Messages.AuthorNotFound);

			return new SuccessDataResult<List<Author>>(authorList);
		}


		[CacheAspect(duration: 10)]
		[SecuredOperation("Author.List")]
		public IDataResult<Author> GetById(int id)
		{
			var result = _authorDal.Get(a => a.Id == id);
			if (result == null)
				return new ErrorDataResult<Author>(Messages.AuthorNotFound);

			return new SuccessDataResult<Author>(result);
		}


		[SecuredOperation("Author.Update")]
		[CacheRemoveAspect(pattern: "IAuthorService.Get")]
		[ValidationAspect(typeof(AuthorValidator))]
		public async Task<IResult> UpdateAsync(AuthorDto authorDto)
		{
			var authorExist = _authorDal.IsExist(a => a.FirstName == authorDto.FirstName && a.LastName == authorDto.LastName);
			if (authorExist) return await Task.FromResult<IResult>(new ErrorResult(Messages.AuthorAlreadyExists));

			var author = GetById(authorDto.Id);
			if (!author.Success) return await Task.FromResult<IResult>(new ErrorResult(author.Message));

			author.Data.FirstName = authorDto.FirstName;
			author.Data.LastName = authorDto.LastName;
			author.Data.Description = authorDto.Description;
			author.Data.AddedAt = DateTime.Now;

			await _authorDal.UpdateAsync(author.Data);

			return new SuccessResult(Messages.AuthorUpdated);
		}


	}
}
