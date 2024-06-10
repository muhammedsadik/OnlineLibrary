using Core.DataAccess;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
	public interface IUserDal : IGenericRepository<User>
	{
		Task<List<OperationClaim>> GetClaims(User user, int companyId = 0);
	}
}
