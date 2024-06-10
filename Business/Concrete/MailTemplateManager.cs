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

//					MailTemplateController oluşturmadım.
namespace Business.Concrete
{
	public class MailTemplateManager : IMailTemplateService
	{
		public readonly IMailTemplateDal _mailTemplateDal;

		public MailTemplateManager(IMailTemplateDal mailTemplateDal)
		{
			_mailTemplateDal = mailTemplateDal;
		}

		public async Task<IResult> AddAsync(MailTemplate mailTemplate)
		{
			await _mailTemplateDal.AddAsync(mailTemplate);
			return new SuccessResult(Messages.MailTemplateAdded);
		}

		public async Task<IResult> DeleteAsync(MailTemplate mailTemplate)
		{
			await _mailTemplateDal.DeleteAsync(mailTemplate);
			return new SuccessResult(Messages.MailTemplateDeleted);
		}

		public IDataResult<MailTemplate> Get(int id)
		{
			return new SuccessDataResult<MailTemplate>(_mailTemplateDal.Get(m => m.Id == id));
		}

		public async Task<IDataResult<List<MailTemplate>>> GetAllAsync(int companyId)
		{
			var mailTemplateList = await _mailTemplateDal.GetAllAsync();
			if (mailTemplateList == null) return new ErrorDataResult<List<MailTemplate>>(Messages.MailTemplateNotFound);

			return new SuccessDataResult<List<MailTemplate>>(mailTemplateList);
		}

		public IDataResult<MailTemplate> GetByTemplateName(string name, int companyId)
		{
			return new SuccessDataResult<MailTemplate>(_mailTemplateDal.Get(m => m.Type == name && m.CompanyId == companyId));
		}

		public async Task<IResult> UpdateAsync(MailTemplate mailTemplate)
		{
			await _mailTemplateDal.UpdateAsync(mailTemplate);
			return new SuccessResult(Messages.MailTemplateUpdated);
		}
	}
}
