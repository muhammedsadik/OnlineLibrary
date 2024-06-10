using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IAuthService
	{
		Task<IDataResult<AccessToken>> UserRegister(UserDto userForRegisterDto);
		Task<IDataResult<AccessToken>> UserLogin(UserForLoginDto userForLoginDto);
		Task<IDataResult<AccessToken>> CreateAccessToken(User user, int companyId);
		IDataResult<User> GetByMailConfirmValue(string value);
		Task<IResult> UpdateAsync(User user);

		//IDataResult<User> UserExist(string email);
		//IDataResult<int> GetCompanyIdByUser(int userId);

	}
}
