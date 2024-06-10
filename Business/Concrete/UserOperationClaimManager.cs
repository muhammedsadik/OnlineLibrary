using Business.Abstract;
using Business.Constans;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class UserOperationClaimManager : IUserOperationClaimService
	{
		private readonly IUserOperationClaimDal _userOperationClaimDal;
		private readonly IUserService _userService;
		private readonly ICompanyService _companyService;
		private readonly IOperationClaimService _operationClaimService;

		public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal, IUserService userService, ICompanyService companyService, IOperationClaimService operationClaimService)
		{
			_userOperationClaimDal = userOperationClaimDal;
			_userService = userService;
			_companyService = companyService;
			_operationClaimService = operationClaimService;
		}

		public async Task<IResult> AddAsync(UserOperationClaimDto userOperationClaimDto)
		{
			var companyExist = GetCompanyByCompanyId(userOperationClaimDto.CompanyId);
			if (!companyExist.Success) return new ErrorResult(companyExist.Message);

			var userExist = GetUserByUserId(userOperationClaimDto.UserId);
			if (!userExist.Success) return new ErrorResult(userExist.Message);

			foreach (var operationClaimId in userOperationClaimDto.OperationClaimIds)
			{
				var operationClaimExist = GetOperationClaimByOperationClaimId(operationClaimId);
				if (!operationClaimExist.Success)
				{
					return new ErrorResult($"The OperationClaim id : {operationClaimId} is not found");
				}

				var userOperationClaimExist = _userOperationClaimDal.Get(x => x.CompanyId == userOperationClaimDto.CompanyId && x.UserId == userOperationClaimDto.UserId && x.OperationClaimId == operationClaimId);
				if (userOperationClaimExist != null)
				{
					return new ErrorResult($"The UserOperationClaim id : {userOperationClaimExist.Id} is already exist");
				}

				UserOperationClaim userOperationClaim = new();
				userOperationClaim.UserId = userOperationClaimDto.UserId;
				userOperationClaim.CompanyId = userOperationClaimDto.CompanyId;
				userOperationClaim.OperationClaimId = operationClaimId;
				userOperationClaim.AddedAt = DateTime.Now;

				await _userOperationClaimDal.AddAsync(userOperationClaim);
			}

			return new SuccessResult(Messages.UserOperationClaimAdded);
		}

		public async Task<IResult> DeleteAsync(UserOperationClaimDto userOperationClaimDto)
		{
			var isUserOperationClaimExist = GetListBySpecificId(userOperationClaimDto);
			if (!isUserOperationClaimExist.Success) return await Task.FromResult<IResult>(new ErrorResult(isUserOperationClaimExist.Message));

			foreach (var userOperationClaim in isUserOperationClaimExist.Data)
			{
				await _userOperationClaimDal.DeleteAsync(userOperationClaim);
			}

			return new SuccessResult(Messages.UserOperationClaimDeleted);
		}

		public async Task<IDataResult<List<UserOperationClaim>>> GetAllAsync()
		{
			var userOperationClaimList = await _userOperationClaimDal.GetAllAsync();
			if (userOperationClaimList == null) return new ErrorDataResult<List<UserOperationClaim>>(Messages.UserOperationClaimNotFound);

			return new SuccessDataResult<List<UserOperationClaim>>(userOperationClaimList);

		}

		public IDataResult<UserOperationClaim> GetByUserOperationClaimId(int id)
		{
			var isUserOperationClaimExist = _userOperationClaimDal.Get(x => x.Id == id);
			if (isUserOperationClaimExist == null) return new ErrorDataResult<UserOperationClaim>(Messages.UserOperationClaimNotFound);

			return new SuccessDataResult<UserOperationClaim>(isUserOperationClaimExist);
		}

		public IDataResult<List<UserOperationClaim>> GetListBySpecificId(UserOperationClaimDto userOperationClaimDto)
		{
			var companyExist = GetCompanyByCompanyId(userOperationClaimDto.CompanyId);
			if (!companyExist.Success) return new ErrorDataResult<List<UserOperationClaim>>(companyExist.Message);

			var userExist = GetUserByUserId(userOperationClaimDto.UserId);
			if (!userExist.Success) return new ErrorDataResult<List<UserOperationClaim>>(userExist.Message);

			List<UserOperationClaim> userOperationClaim = new();
			foreach (var userOperationClaimId in userOperationClaimDto.OperationClaimIds)
			{
				var result = GetOperationClaimByOperationClaimId(userOperationClaimId);
				if (!result.Success)
				{
					return new ErrorDataResult<List<UserOperationClaim>>($"The OperationClaim id : {userOperationClaimId} is not found");
				}

				var userOperationClaimExist = _userOperationClaimDal.Get(x => x.CompanyId == userOperationClaimDto.CompanyId && x.UserId == userOperationClaimDto.UserId && x.OperationClaimId == userOperationClaimId);
				if (userOperationClaimExist == null)
				{
					return new ErrorDataResult<List<UserOperationClaim>>($"The user id : {userOperationClaimDto.UserId} , company id : {userOperationClaimDto.CompanyId} , UserOperationClaim id {userOperationClaimId} is not found");
				}

				userOperationClaim.Add(userOperationClaimExist);
			}

			return new SuccessDataResult<List<UserOperationClaim>>(userOperationClaim);
		}
		
		public async Task<IResult> UpdateAsync(UserOperationClaimUpdateDto userOperationClaimUpdateDto)
		{
			var useroperationClaimExist = _userOperationClaimDal.Get(x => x.Id == userOperationClaimUpdateDto.Id);
			if (useroperationClaimExist == null) return await Task.FromResult<IResult>(new ErrorResult(Messages.UserOperationClaimNotFound));

			var companyExist = GetCompanyByCompanyId(userOperationClaimUpdateDto.CompanyId);
			if (!companyExist.Success) return await Task.FromResult<IResult>(new ErrorResult(companyExist.Message));//burayı butün çiftler için yap

			var userExist = GetUserByUserId(userOperationClaimUpdateDto.UserId);
			if (!userExist.Success) return await Task.FromResult<IResult>(new ErrorResult(userExist.Message));//burayı butün çiftler için yap

			var operationClaimExist = GetOperationClaimByOperationClaimId(userOperationClaimUpdateDto.OperationClaimId);
			if (!operationClaimExist.Success) return await Task.FromResult<IResult>(new ErrorResult(operationClaimExist.Message));

			useroperationClaimExist.CompanyId = userOperationClaimUpdateDto.CompanyId;
			useroperationClaimExist.UserId = userOperationClaimUpdateDto.UserId;
			useroperationClaimExist.OperationClaimId = userOperationClaimUpdateDto.OperationClaimId;
			useroperationClaimExist.AddedAt = DateTime.Now;

			await _userOperationClaimDal.UpdateAsync(useroperationClaimExist);

			return new SuccessResult(Messages.UserOperationClaimUpdated);

		}

		public IDataResult<List<User>> GetUserListByOperationClaimId(int operationClaimId)
		{
			var operationClaimExist = GetOperationClaimByOperationClaimId(operationClaimId);
			if (!operationClaimExist.Success) return new ErrorDataResult<List<User>>(operationClaimExist.Message);

			var userOperationClaimExist = GetUserOperationClaimListByoperationClaimId(operationClaimId);
			if (!userOperationClaimExist.Success) return new ErrorDataResult<List<User>>(userOperationClaimExist.Message);

			var userList = new List<User>();
			foreach (var userOperationClaim in userOperationClaimExist.Data)
			{
				var user = GetUserByUserId(userOperationClaim.UserId);
				if (!user.Success) return new ErrorDataResult<List<User>>(user.Message);

				userList.Add(user.Data);
			}

			return new SuccessDataResult<List<User>>(userList);
		}

		public IDataResult<List<UserOperationClaim>> GetUserOperationClaimListByoperationClaimId(int operationClaimId)
		{
			var operationClaimExist = GetOperationClaimByOperationClaimId(operationClaimId);
			if (!operationClaimExist.Success) return new ErrorDataResult<List<UserOperationClaim>>(operationClaimExist.Message);

			var userOperationClaim = _userOperationClaimDal.Where(x => x.OperationClaimId == operationClaimId).ToList();
			if (userOperationClaim.Count == 0) return new ErrorDataResult<List<UserOperationClaim>>(Messages.UserOperationClaimListNotFound);

			return new SuccessDataResult<List<UserOperationClaim>>(userOperationClaim);
		}
		
		public IDataResult<List<UserOperationClaim>> GetUserOperationClaimListByuserId(int userId)
		{
			var userExist = GetUserByUserId(userId);
			if (!userExist.Success) return new ErrorDataResult<List<UserOperationClaim>>(userExist.Message);

			var userOperationClaim = _userOperationClaimDal.Where(x => x.UserId == userId).ToList();
			if (userOperationClaim.Count == 0) return new ErrorDataResult<List<UserOperationClaim>>(Messages.UserOperationClaimListNotFound);

			return new SuccessDataResult<List<UserOperationClaim>>(userOperationClaim);
		}
		
		public IDataResult<List<OperationClaim>> GetOperationClaimListByUserId(int userId)
		{
			var userOperationClaimExist = GetUserOperationClaimListByuserId(userId);
			if (!userOperationClaimExist.Success) return new ErrorDataResult<List<OperationClaim>>(userOperationClaimExist.Message);

			List<OperationClaim> operationClaimList = new();
			foreach (var userOperationClaim in userOperationClaimExist.Data)
			{
				var operationClaim = GetOperationClaimByOperationClaimId(userOperationClaim.OperationClaimId);
				if(!operationClaim.Success) return new ErrorDataResult<List<OperationClaim>>(operationClaim.Message);

				operationClaimList.Add(operationClaim.Data);
			}

			return new SuccessDataResult<List<OperationClaim>>(operationClaimList);
		}


		private IDataResult<Company> GetCompanyByCompanyId(int authorId)
		{
			var companyExist = _companyService.GetById(authorId);
			if (!companyExist.Success) return new ErrorDataResult<Company>(companyExist.Message);

			return new SuccessDataResult<Company>(companyExist.Data);
		}

		private IDataResult<User> GetUserByUserId(int userId)
		{
			var userExist = _userService.GetById(userId);
			if (!userExist.Success) return new ErrorDataResult<User>(userExist.Message);

			return new SuccessDataResult<User>(userExist.Data);
		}		

		private IDataResult<OperationClaim> GetOperationClaimByOperationClaimId(int userId)
		{
			var operationClaimExist = _operationClaimService.GetById(userId);
			if (!operationClaimExist.Success) return new ErrorDataResult<OperationClaim>(operationClaimExist.Message);

			return new SuccessDataResult<OperationClaim>(operationClaimExist.Data);
		}

		public IDataResult<List<UserOperationClaim>> GetUserOperationClaimListByCompanyId(int companyId)
		{
			var companyExist = GetCompanyByCompanyId(companyId);
			if (!companyExist.Success) return new ErrorDataResult<List<UserOperationClaim>>(companyExist.Message);

			var userOperationClaim = _userOperationClaimDal.Where(x => x.CompanyId == companyId).ToList();
			if (userOperationClaim.Count == 0) return new ErrorDataResult<List<UserOperationClaim>>(Messages.UserOperationClaimListNotFound);

			return new SuccessDataResult<List<UserOperationClaim>>(userOperationClaim);
		}


		public async Task<IResult> DeleteFromUserOperationClaimTableAsync(int companyId)
		{
			var isUserOperationClaimExist = GetUserOperationClaimListByCompanyId(companyId);
			if (!isUserOperationClaimExist.Success) return await Task.FromResult<IResult>(new ErrorResult(isUserOperationClaimExist.Message));

			foreach (var userOperationClaim in isUserOperationClaimExist.Data)
			{
				await _userOperationClaimDal.DeleteAsync(userOperationClaim);
			}

			return new SuccessResult(Messages.UserOperationClaimDeleted);
		}
	}
}
