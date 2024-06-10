using Business.Abstract;
using Business.Constans;
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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class UserCompanyManager : IUserCompanyService
	{
		private readonly IUserCompanyDal _userCompanyDal;
		private readonly IUserService _userService;
		private readonly ICompanyService _companyService;

		public UserCompanyManager(IUserCompanyDal userCompanyDal, IUserService userService, ICompanyService companyService)
		{
			_userCompanyDal = userCompanyDal;
			_userService = userService;
			_companyService = companyService;
		}

		public async Task<IResult> AddAsync(UsersCompanyDto usersCompanyDto)
		{
			var companyExist = GetCompanyByCompanyId(usersCompanyDto.CompanyId);
			if (!companyExist.Success) return new ErrorResult(companyExist.Message);


			foreach (var userId in usersCompanyDto.UserId)
			{
				var result = GetUserByUserId(userId);
				if (!result.Success)
				{
					return new ErrorResult($"The user id : {userId} is not found");
				}

				var userCompanyExist = _userCompanyDal.Get(x => x.CompanyId == usersCompanyDto.CompanyId && x.UserId == userId);
				if (userCompanyExist != null)
				{
					return new ErrorResult($"The Company and user id : {userCompanyExist.Id} is already exist");
				}

				UserCompany userCompany = new();
				userCompany.UserId = userId;
				userCompany.CompanyId = usersCompanyDto.CompanyId;
				userCompany.AddedAt = DateTime.Now;

				await _userCompanyDal.AddAsync(userCompany);
			}

			return new SuccessResult(Messages.UserCompanyAdded);
		}

		public async Task<IResult> DeleteAsync(UsersCompanyDto usersCompanyDto)
		{
			var isUserCompanyExist = GetListByUsersIdAndCompanyId(usersCompanyDto);
			if (!isUserCompanyExist.Success) return await Task.FromResult<IResult>(new ErrorResult(isUserCompanyExist.Message));

			foreach (var usersCompany in isUserCompanyExist.Data)
			{
				await _userCompanyDal.DeleteAsync(usersCompany);
			}

			return new SuccessResult(Messages.UserCompanyDeleted);
		}

		public async Task<IDataResult<List<UserCompany>>> GetAllAsync()
		{
			var usersCompanyList = await _userCompanyDal.GetAllAsync();
			if (usersCompanyList == null) return new ErrorDataResult<List<UserCompany>>(Messages.AuthorBookNotFound);

			return new SuccessDataResult<List<UserCompany>>(usersCompanyList);
		}

		public IDataResult<UserCompany> GetUserCompanyByUserCompanyId(int id)
		{
			var isUserCompanyExist = _userCompanyDal.Get(x => x.Id == id);
			if (isUserCompanyExist == null) return new ErrorDataResult<UserCompany>(Messages.UserCompanyNotFound);

			return new SuccessDataResult<UserCompany>(isUserCompanyExist);
		}

		public IDataResult<List<UserCompany>> GetListByUsersIdAndCompanyId(UsersCompanyDto usersCompanyDto)
		{
			var companyExist = GetCompanyByCompanyId(usersCompanyDto.CompanyId);
			if (!companyExist.Success) return new ErrorDataResult<List<UserCompany>>(companyExist.Message);

			var userCompany = new List<UserCompany>();
			foreach (var userId in usersCompanyDto.UserId)
			{
				var result = GetUserByUserId(userId);
				if (!result.Success)
				{
					return new ErrorDataResult<List<UserCompany>>($"The user id : {userId} is not found");
				}

				var userCompanyExist = _userCompanyDal.Get(x => x.CompanyId == usersCompanyDto.CompanyId && x.UserId == userId);
				if (userCompanyExist == null)
				{
					return new ErrorDataResult<List<UserCompany>>($"The userid : {userId} , company id : {usersCompanyDto.CompanyId} is not found");
				}

				userCompany.Add(userCompanyExist);
			}

			return new SuccessDataResult<List<UserCompany>>(userCompany);
		}

		public IDataResult<UserCompany> GetUserCompanyByUserId(int userId)
		{
			var userCompany = _userCompanyDal.Get(x => x.UserId == userId);
			if (userCompany == null) return new ErrorDataResult<UserCompany>(Messages.UserCompanyNotFound);

			return new SuccessDataResult<UserCompany>(userCompany);
		}

		public IDataResult<List<UserCompany>> GetUserCompanyListByCompanyId(int companyId)
		{
			var companyExist = GetCompanyByCompanyId(companyId);
			if (!companyExist.Success) return new ErrorDataResult<List<UserCompany>>(companyExist.Message);

			var companyList = _userCompanyDal.Where(x => x.CompanyId == companyId).ToList();
			if (companyList.Count == 0) return new ErrorDataResult<List<UserCompany>>(Messages.BookNotFound);

			return new SuccessDataResult<List<UserCompany>>(companyList);
		}

		public async Task<IResult> UpdateAsync(UserCompanyUpdateDto userCompanyUpdateDto)
		{
			var isUserCompanyExist = _userCompanyDal.Get(x => x.Id == userCompanyUpdateDto.Id);
			if (isUserCompanyExist == null) return await Task.FromResult<IResult>(new ErrorResult(Messages.UserCompanyNotFound));

			var companyExist = GetCompanyByCompanyId(userCompanyUpdateDto.CompanyId);
			if(!companyExist.Success) return await Task.FromResult<IResult>(new ErrorResult(companyExist.Message));//burayı butün çiftler için yap

			var userExist = GetUserByUserId(userCompanyUpdateDto.UserId);
			if (!userExist.Success) return await Task.FromResult<IResult>(new ErrorResult(userExist.Message));//burayı butün çiftler için yap

			isUserCompanyExist.CompanyId = userCompanyUpdateDto.CompanyId;
			isUserCompanyExist.UserId = userCompanyUpdateDto.UserId;
			isUserCompanyExist.AddedAt = DateTime.Now;

			await _userCompanyDal.UpdateAsync(isUserCompanyExist);

			return new SuccessResult(Messages.UserCompanyUpdated);
		}

		public IDataResult<List<User>> GetUserListByCompanyId(int companyId)
		{
			var companyExist = GetCompanyByCompanyId(companyId);
			if (!companyExist.Success) return new ErrorDataResult<List<User>>(companyExist.Message);

			var userCompanyExist = GetUserCompanyListByCompanyId(companyId);
			if (!userCompanyExist.Success) return new ErrorDataResult<List<User>>(userCompanyExist.Message);

			var userList = new List<User>();
			foreach (var userCompany in userCompanyExist.Data)
			{
				var user = GetUserByUserId(userCompany.UserId);
				if (!user.Success) return new ErrorDataResult<List<User>>(user.Message);

				userList.Add(user.Data);
			}

			return new SuccessDataResult<List<User>>(userList);
		}

		public IDataResult<Company> GetCompanyByUserId(int userId)
		{
			var userExist = GetUserByUserId(userId);
			if (!userExist.Success) return new ErrorDataResult<Company>(userExist.Message);

			var userCompany = GetUserCompanyByUserId(userId);
			if (!userCompany.Success) return new ErrorDataResult<Company>(userCompany.Message);

			var company = GetCompanyByCompanyId(userCompany.Data.CompanyId).Data;
			if (company == null) return new ErrorDataResult<Company>(Messages.CompanyNotFound);

			return new SuccessDataResult<Company>(company);
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

		public async Task<IResult> DeleteFromUserCompanyTableAsync(int companyId)
		{
			var isUserCompanyExist = GetUserCompanyListByCompanyId(companyId);
			if (!isUserCompanyExist.Success) return await Task.FromResult<IResult>(new ErrorResult(isUserCompanyExist.Message));

			foreach (var usersCompany in isUserCompanyExist.Data)
			{
				await _userCompanyDal.DeleteAsync(usersCompany);
			}

			return new SuccessResult(Messages.UserCompanyDeleted);
		}
	}
}
