using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IMailTemplateService
	{
		Task<IResult> AddAsync(MailTemplate mailTemplate);
		Task<IResult> DeleteAsync(MailTemplate mailTemplate);
		Task<IResult> UpdateAsync(MailTemplate mailTemplate);
		IDataResult<MailTemplate> Get(int id);
		IDataResult<MailTemplate> GetByTemplateName(string name, int companyId);
		Task<IDataResult<List<MailTemplate>>> GetAllAsync(int companyId);
	}
}
