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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class OperationClaimManager : IOperationClaimService
	{
		private readonly IOperationClaimDal _operationClaimDal;

		public OperationClaimManager(IOperationClaimDal operationClaimDal)
		{
			_operationClaimDal = operationClaimDal;
		}


		[SecuredOperation("OperationClaim.Create")]
		[CacheRemoveAspect(pattern: "IOperationClaimDal.Get")]
		[ValidationAspect(typeof(OperationClaimValidator))]
		public async Task<IResult> AddAsync(OperationClaimDto operationClaimDto)
		{
			var operationClaimExist = _operationClaimDal.IsExist(c => c.Name == operationClaimDto.Name);
			if (operationClaimExist) return await Task.FromResult<IResult>(new ErrorResult(Messages.OperationClaimAlreadyExists));

			var operationClaim = new OperationClaim
			{
				Name = operationClaimDto.Name,
				AddedAt = DateTime.Now,
			};

			await _operationClaimDal.AddAsync(operationClaim);

			return new SuccessResult(Messages.OperationClaimAdded);
		}


		[SecuredOperation("OperationClaim.Delete")]
		[CacheRemoveAspect(pattern: "IOperationClaimDal.Get")]
		public async Task<IResult> DeleteAsync(int operationClaimId)
		{
			var operationClaimExist = GetById(operationClaimId);
			if (operationClaimExist.Data == null) return new ErrorResult(Messages.OperationClaimNotFound);

			await _operationClaimDal.DeleteAsync(operationClaimExist.Data);

			return new SuccessResult(Messages.OperationClaimDeleted);
		}


		[SecuredOperation("OperationClaim.List")]
		[CacheAspect(duration: 10)]
		public async Task<IDataResult<List<OperationClaim>>> GetAllAsync()
		{
			var operationClaimList = await _operationClaimDal.GetAllAsync();
			if (operationClaimList == null) return new ErrorDataResult<List<OperationClaim>>(Messages.OperationClaimNotFound);

			return new SuccessDataResult<List<OperationClaim>>(operationClaimList);
		}


		[SecuredOperation("OperationClaim.List")]
		public IDataResult<OperationClaim> GetById(int id)
		{
			var result = _operationClaimDal.Get(a => a.Id == id);
			if (result == null)
				return new ErrorDataResult<OperationClaim>(Messages.OperationClaimNotFound);

			return new SuccessDataResult<OperationClaim>(result);
		}


		[SecuredOperation("OperationClaim.Update")]
		[CacheRemoveAspect(pattern: "IOperationClaimDal.Get")]
		[ValidationAspect(typeof(OperationClaimValidator))]
		public async Task<IResult> UpdateAsync(OperationClaimDto operationClaimDto)
		{
			var operationClaimExist = GetById(operationClaimDto.Id).Data;
			if (operationClaimExist == null) return await Task.FromResult<IResult>(new ErrorResult(Messages.OperationClaimNotFound));

			operationClaimExist.Name = operationClaimDto.Name;
			operationClaimExist.AddedAt = DateTime.Now;

			await _operationClaimDal.UpdateAsync(operationClaimExist);

			return new SuccessResult(Messages.OperationClaimUpdated);
		}


	}
}
