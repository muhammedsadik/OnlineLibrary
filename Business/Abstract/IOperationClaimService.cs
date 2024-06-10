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
	public interface IOperationClaimService
	{
		Task<IResult> AddAsync(OperationClaimDto operationClaimDto);
		Task<IResult> UpdateAsync(OperationClaimDto operationClaimDto);
		Task<IResult> DeleteAsync(int operationClaimId);
		IDataResult<OperationClaim> GetById(int id);
		Task<IDataResult<List<OperationClaim>>> GetAllAsync();
	}
}
