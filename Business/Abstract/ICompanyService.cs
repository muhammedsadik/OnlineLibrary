using Core.Entities.Concrete;
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
	public interface ICompanyService
	{
		Task<IResult> AddAsync(CompanyDto companyDto);
		Task<IResult> UpdateAsync(CompanyDto companyDto);
		Task<IResult> DeleteAsync(int companyId);
		IDataResult<Company> GetById(int id);
		Task<IDataResult<List<Company>>> GetAllAsync();
		
	}
}
