﻿using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface ICompanyCommanService
	{
		Task<IResult> DeleteFromUserCompanyTableAsync(int companyId);
		Task<IResult> DeleteFromUserOperationClaimTableAsync(int companyId);


	}
}
