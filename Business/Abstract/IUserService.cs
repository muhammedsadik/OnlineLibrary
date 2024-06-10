using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IUserService
	{
		Task<IDataResult<User>> AddAsync(UserDto userDto);
		Task<IResult> UpdateByUserDtoAsync(UserDto userDto);
		Task<IResult> UpdateAsync(User user);
		Task<IResult> DeleteAsync(int UserId);
		IDataResult<User> GetById(int id);
		Task<IDataResult<List<User>>> GetAllAsync();
		IDataResult<User> GetByMail(string email);
		IDataResult<User> GetByMailConfirmValue(string mailConfirmValue);
		Task<IDataResult<List<OperationClaim>>> GetUserClaims(User user, int companyId);
	}
}
