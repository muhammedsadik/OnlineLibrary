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
	public interface ICategoryService
	{
		Task<IResult> AddAsync(CategoryDto categoryDto);
		Task<IResult> UpdateAsync(CategoryDto categoryDto);
		Task<IResult> DeleteAsync(int categoryId);
		IDataResult<Category> GetById(int id);
		Task<IDataResult<List<Category>>> GetAllAsync();
	}
}
