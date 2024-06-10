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
	public interface IUserCompanyService
	{
		Task<IResult> AddAsync(UsersCompanyDto usersCompanyDto);
		Task<IResult> UpdateAsync(UserCompanyUpdateDto userCompanyUpdateDto);
		Task<IResult> DeleteAsync(UsersCompanyDto usersCompanyDto);
		Task<IResult> DeleteFromUserCompanyTableAsync(int companyId);
		IDataResult<UserCompany> GetUserCompanyByUserCompanyId(int id);
		Task<IDataResult<List<UserCompany>>> GetAllAsync();
		IDataResult<List<UserCompany>> GetListByUsersIdAndCompanyId(UsersCompanyDto usersCompanyDto);
	
		IDataResult<List<UserCompany>> GetUserCompanyListByCompanyId(int companyId);
		IDataResult<UserCompany> GetUserCompanyByUserId(int userId);
		IDataResult<Company> GetCompanyByUserId(int userId);
		IDataResult<List<User>> GetUserListByCompanyId(int companyId);
	}
}
