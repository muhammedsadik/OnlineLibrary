using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
	public class UserDal : EfGenericRepositoryBase<User, LibContext>, IUserDal
	{
		public async Task<List<OperationClaim>> GetClaims(User user, int companyId)
		{
			using (var context = new LibContext())
			{
				var result =
					from operationClaim in context.OperationClaims
					join userOperationClaim in context.UserOperationClaims 
					on operationClaim.Id equals userOperationClaim.OperationClaimId
					where userOperationClaim.UserId == user.Id && userOperationClaim.CompanyId == companyId
					select new OperationClaim
					{
						Id = operationClaim.Id,
						Name = operationClaim.Name,
						IsActive = operationClaim.IsActive
					};

				return await result.ToListAsync();
			}
		}
	}
}
