using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IUserOperationClaimService
	{
		Task<IResult> AddAsync(UserOperationClaimDto userOperationClaimDto);
		Task<IResult> UpdateAsync(UserOperationClaimUpdateDto userOperationClaimUpdateDto);
		Task<IResult> DeleteAsync(UserOperationClaimDto userOperationClaimDto);
		Task<IResult> DeleteFromUserOperationClaimTableAsync(int companyId);
		IDataResult<List<UserOperationClaim>> GetUserOperationClaimListByCompanyId(int companyId);
		IDataResult<UserOperationClaim> GetByUserOperationClaimId(int id); 
		Task<IDataResult<List<UserOperationClaim>>> GetAllAsync();
		IDataResult<List<UserOperationClaim>> GetListBySpecificId(UserOperationClaimDto userOperationClaimDto);
		IDataResult<List<UserOperationClaim>> GetUserOperationClaimListByuserId(int userId);
		IDataResult<List<OperationClaim>> GetOperationClaimListByUserId(int userId);
		IDataResult<List<User>> GetUserListByOperationClaimId(int operationClaimId);
		IDataResult<List<UserOperationClaim>> GetUserOperationClaimListByoperationClaimId(int operationClaimId); 

	}
}
