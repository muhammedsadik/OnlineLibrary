using Business.Abstract;
using Business.Constans;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class MailParameterManager : IMailParameterService
	{
		private readonly IMailParameterDal _mailParameterDal;

		public MailParameterManager(IMailParameterDal mailParameterDal)
		{
			_mailParameterDal = mailParameterDal;
		}

		public IDataResult<MailParameter> Get(int companyId)
		{
			var mailParemeter = _mailParameterDal.Get(m=>m.CompanyId == companyId);
			if (mailParemeter == null) return new ErrorDataResult<MailParameter>(Messages.MailParameterNotFound);

			return new SuccessDataResult<MailParameter>(mailParemeter);
		}

		public async Task<IResult> Update(MailParameter mailParameter)
		{

			var result = Get(mailParameter.CompanyId).Data;
			if (result == null)
			{
				await _mailParameterDal.AddAsync(mailParameter);
			}
			else
			{
				result.SMTP = mailParameter.SMTP;
				result.Port = mailParameter.Port;
				result.SSL = mailParameter.SSL;
				result.Email = mailParameter.Email;
				result.EmailPassword = mailParameter.EmailPassword;

				await _mailParameterDal.UpdateAsync(result);
			}

			return new SuccessResult(Messages.MailParameterUpdated);
		}
	}
}
