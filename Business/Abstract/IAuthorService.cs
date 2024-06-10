using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IAuthorService
	{
		Task<IResult> AddAsync(AuthorDto authorDto);
		Task<IResult> UpdateAsync(AuthorDto authorDto);
		Task<IResult> DeleteAsync(int authorId);
		IDataResult<Author> GetById(int id);
		Task<IDataResult<List<Author>>> GetAllAsync();
	}
}
