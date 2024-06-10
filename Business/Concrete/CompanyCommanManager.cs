using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class CompanyCommanManager : ICompanyCommanService
	{
		private readonly IUserCompanyService _userCompanyService;
		private readonly IUserOperationClaimService _userOperationClaimService;

		public CompanyCommanManager(IUserCompanyService userCompanyService, IUserOperationClaimService userOperationClaimService)
		{
			_userCompanyService = userCompanyService;
			_userOperationClaimService = userOperationClaimService;
		}

		public async Task<IResult> DeleteFromUserCompanyTableAsync(int companyId)
		{
			var result = await _userCompanyService.DeleteFromUserCompanyTableAsync(companyId);

			return result;
		}

		public async Task<IResult> DeleteFromUserOperationClaimTableAsync(int companyId)
		{
			var result = await _userOperationClaimService.DeleteFromUserOperationClaimTableAsync(companyId);

			return result;
		}

	}
}
