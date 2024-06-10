using Business.Abstract;
using Business.BusinessAspect;
using Business.Constans;
using Business.ValidationRules.FluentValidation;
using Core.Aspect.Autofac.Caching;
using Core.Aspect.Autofac.Logging;
using Core.Aspect.Autofac.Performance;
using Core.Aspect.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Security.Hashing;
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
	public class UserManager : IUserService
	{
		private readonly IUserDal _userDal;

		public UserManager(IUserDal userDal)
		{
			_userDal = userDal;
		}


		[SecuredOperation("User.Create")]
		[CacheRemoveAspect(pattern: "IUserDal.Get")]
		[ValidationAspect(typeof(UserValidator))]
		public async Task<IDataResult<User>> AddAsync(UserDto userDto)
		{
			var userExist = _userDal.IsExist(u => u.Email== userDto.Email);
			if (userExist) return await Task.FromResult<IDataResult<User>>(new ErrorDataResult<User>(Messages.UserAlreadyExists));

			byte[] passwordHash, passwordSalt;

			HashingHelper.CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

			var user = new User
			{
				FirstName = userDto.FirstName,
				LastName = userDto.LastName,
				Email = userDto.Email,
				AddedAt = DateTime.Now,
				MailConfirmValue = Guid.NewGuid().ToString(),
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt
			};
			await _userDal.AddAsync(user);

			return new SuccessDataResult<User>(user, Messages.UserAdded);//here instead of user => GetByMailConfirmValue(user.MailConfirmValue)
		}


		[SecuredOperation("User.Delete")]
		[CacheRemoveAspect(pattern: "IUserDal.Get")]
		public async Task<IResult> DeleteAsync(int userId)
		{
			var userExist = GetById(userId);
			if (userExist == null) return new ErrorResult(Messages.UserNotFound);

			await _userDal.DeleteAsync(userExist.Data);

			return new SuccessResult(Messages.UserDeleted);
		}


		//[PerformanceAspect(5)]
		[SecuredOperation("User.List")]
		[CacheAspect(duration: 10)]
		public async Task<IDataResult<List<User>>> GetAllAsync()
		{
			//Thread.Sleep(4000); // PerformanceAspect Deneme

			var userList = await _userDal.GetAllAsync();
			if (userList == null) return new ErrorDataResult<List<User>>(Messages.UserNotFound);

			return new SuccessDataResult<List<User>>(userList);
		}

		[SecuredOperation("User.List")]
		public IDataResult<User> GetById(int id)
		{
			var result = _userDal.Get(a => a.Id == id);
			if (result == null)
				return new ErrorDataResult<User>(Messages.UserNotFound);

			return new SuccessDataResult<User>(result);
		}


		[SecuredOperation("User.List")]
		public IDataResult<User> GetByMail(string email)
		{
			var user =  _userDal.Get(u => u.Email == email);
			if (user == null)
				return new ErrorDataResult<User>(Messages.UserNotFound);
			

			return new SuccessDataResult<User>(user,Messages.UserAlreadyExists);
		}


		[SecuredOperation("User.List")]
		public IDataResult<User> GetByMailConfirmValue(string mailConfirmValue)
		{
			var user = _userDal.Get(x=> x.MailConfirmValue == mailConfirmValue);
			if (user == null) return new ErrorDataResult<User>(Messages.MailConfirmValueNotFound);

			return new SuccessDataResult<User>(user);
		}


		[SecuredOperation("User.List")]
		public async Task<IDataResult<List<OperationClaim>>> GetUserClaims(User user, int companyId)
		{
			var userExist = GetById(user.Id).Data;
			if (userExist == null) return await Task.FromResult<IDataResult<List<OperationClaim>>>(new SuccessDataResult<List<OperationClaim>>(Messages.UserNotFound));

			var result = await _userDal.GetClaims(user, companyId);

			return new SuccessDataResult<List<OperationClaim>>(result);
		}


		[SecuredOperation("User.Update")]
		[CacheRemoveAspect(pattern: "IUserDal.Get")]
		[ValidationAspect(typeof(UserValidator))]
		public async Task<IResult> UpdateByUserDtoAsync(UserDto userDto)
		{ 
			var userExist = GetById(userDto.Id).Data;
			if (userExist == null) return await Task.FromResult<IResult>(new ErrorResult(Messages.UserNotFound));

			userExist.FirstName = userDto.FirstName;
			userExist.LastName = userDto.LastName;
			userExist.Email = userDto.Email;
			userExist.AddedAt = DateTime.Now;

			await _userDal.UpdateAsync(userExist);

			return new SuccessResult(Messages.UserUpdated);
		}


		[SecuredOperation("User.Update")]
		[CacheRemoveAspect(pattern: "IUserDal.Get")]
		[ValidationAspect(typeof(UserValidator))]
		public async Task<IResult> UpdateAsync(User user)
		{ 
			var userExist = GetById(user.Id).Data;
			if (userExist == null) return await Task.FromResult<IResult>(new ErrorResult(Messages.UserNotFound));

			userExist.FirstName = user.FirstName;
			userExist.LastName = user.LastName;
			userExist.Email = user.Email;
			userExist.AddedAt = DateTime.Now;
			userExist.IsMailConfirm = user.IsMailConfirm;
			userExist.MailConfirmDate = user.MailConfirmDate;

			await _userDal.UpdateAsync(userExist);

			return new SuccessResult(Messages.UserUpdated);
		}

	}
}
