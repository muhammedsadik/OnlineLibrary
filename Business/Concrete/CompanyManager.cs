using Business.Abstract;
using Business.BusinessAspect;
using Business.Constans;
using Business.ValidationRules.FluentValidation;
using Core.Aspect.Autofac.Caching;
using Core.Aspect.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class CompanyManager : ICompanyService
	{
		private readonly ICompanyDal _companyDal;

		public CompanyManager(ICompanyDal companyDal)
		{
			_companyDal = companyDal;
		}


		[SecuredOperation("Company.Create")]
		[CacheRemoveAspect(pattern: "ICompanyDal.Get")]
		[ValidationAspect(typeof(CompanyValidator))]
		public async Task<IResult> AddAsync(CompanyDto companyDto)
		{
			var companyExist = _companyDal.IsExist(c => c.Name == companyDto.Name);
			if (companyExist) return await Task.FromResult<IResult>(new ErrorResult(Messages.CompanyAlreadyExists));

			var company = new Company
			{
				Name = companyDto.Name,
				Address = companyDto.Address,
				AddedAt = DateTime.Now
			};
			await _companyDal.AddAsync(company);

			return new SuccessResult(Messages.CompanyAdded);
		}


		[SecuredOperation("Company.Delete")]
		[CacheRemoveAspect(pattern: "ICompanyDal.Get")]
		public async Task<IResult> DeleteAsync(int companyId)
		{
			var companyExist = GetById(companyId);
			if (companyExist == null) return new ErrorResult(Messages.CompanyNotFound);

			await _companyDal.DeleteAsync(companyExist.Data);

			return new SuccessResult(Messages.CompanyDeleted);
		}


		[SecuredOperation("Company.List")]
		[CacheAspect(duration: 10)]
		public async Task<IDataResult<List<Company>>> GetAllAsync()
		{
			var companyExist = await _companyDal.GetAllAsync();
			if (companyExist == null) return new ErrorDataResult<List<Company>>(Messages.CompanyNotFound);

			return new SuccessDataResult<List<Company>>(companyExist);
		}


		[SecuredOperation("Company.List")]
		public IDataResult<Company> GetById(int id)
		{
			var result = _companyDal.Get(c => c.Id == id);
			if (result == null)
				return new ErrorDataResult<Company>(Messages.CompanyNotFound);

			return new SuccessDataResult<Company>(result);
		}


		[SecuredOperation("Company.Update")]
		[CacheRemoveAspect(pattern: "ICompanyDal.Get")]
		[ValidationAspect(typeof(CompanyValidator))]
		public async Task<IResult> UpdateAsync(CompanyDto companyDto)
		{
			var companyExist = _companyDal.IsExist(c => c.Name == companyDto.Name);
			if (companyExist) return await Task.FromResult<IResult>(new ErrorResult(Messages.CompanyAlreadyExists));

			var company = GetById(companyDto.Id).Data;
			if (company == null) return await Task.FromResult<IResult>(new ErrorResult(Messages.CompanyNotFound));

			company.Name = companyDto.Name;
			company.Address = companyDto.Address;
			company.AddedAt = DateTime.Now;

			await _companyDal.UpdateAsync(company);

			return new SuccessResult(Messages.CompanyUpdated);
		}





	}
}
