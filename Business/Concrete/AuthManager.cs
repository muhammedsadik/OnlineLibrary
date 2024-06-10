using Business.Abstract;
using Business.Constans;
using Core.Aspect.Autofac.Transaction;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class AuthManager : IAuthService
	{
		private readonly IUserService _userService;
		private readonly ITokenHelper _tokenHelper;
		private readonly IUserCompanyService _userCompanyService;
		private readonly ICompanyService _CompanService;
		private readonly IMailService _mailService;
		private readonly IMailTemplateService _mailTemplateService;
		private readonly IMailParameterService _mailParameterService;


		public AuthManager(IUserService userService, ITokenHelper tokenHelper, IUserCompanyService userCompanyService, ICompanyService companyService, IMailService mailService, IMailParameterService mailParameterService, IMailTemplateService mailTemplateService)
		{
			_userService = userService;
			_tokenHelper = tokenHelper;
			_userCompanyService = userCompanyService;
			_CompanService = companyService;
			_mailService = mailService;
			_mailParameterService = mailParameterService;
			_mailTemplateService = mailTemplateService;
		}


		public async Task<IDataResult<AccessToken>> UserRegister(UserDto userDto)
		{
			var userExist = UserExist(userDto.Email);
			if (userExist.Success) return new ErrorDataResult<AccessToken>(Messages.UserAlreadyExists);

			var user = await _userService.AddAsync(userDto);


			var tokenResult = await CreateAccessToken(user.Data, userDto.CompanyId);

			if (userDto.CompanyId != 0)
			{
				var companyExist = _CompanService.GetById(userDto.CompanyId);
				if (!companyExist.Success) return new ErrorDataResult<AccessToken>(tokenResult.Data, Messages.UserRegistered + "  " + companyExist.Message);
			}

			SendConfirmEmailWithTemplate(user.Data, userDto.CompanyId);

			return new SuccessDataResult<AccessToken>(tokenResult.Data, Messages.UserRegistered);
		}

		public async Task<IDataResult<AccessToken>> UserLogin(UserForLoginDto userForLoginDto)
		{
			var userExist = UserExist(userForLoginDto.Email);
			if (!userExist.Success) return await Task.FromResult<IDataResult<AccessToken>>(new ErrorDataResult<AccessToken>(Messages.UserNotFound));

			if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userExist.Data.PasswordHash, userExist.Data.PasswordSalt))
				return new ErrorDataResult<AccessToken>(Messages.PasswordError);

			var companyId = GetCompanyIdByUser(userExist.Data.Id);

			var tokenResult = await CreateAccessToken(userExist.Data, companyId.Data);
			if (!tokenResult.Success) return await Task.FromResult<IDataResult<AccessToken>>(new ErrorDataResult<AccessToken>(tokenResult.Message));

			return new SuccessDataResult<AccessToken>(tokenResult.Data, Messages.SuccessfulLogin);
		}

		public async Task<IDataResult<AccessToken>> CreateAccessToken(User user, int companyId)
		{
			var operationClaims = await _userService.GetUserClaims(user, companyId);

			var token = _tokenHelper.CreateToken(user, operationClaims.Data, companyId);

			return new SuccessDataResult<AccessToken>(token);
		}

		private IDataResult<User> UserExist(string email)
		{
			var userExist = _userService.GetByMail(email);
			if (!userExist.Success) return new ErrorDataResult<User>(userExist.Message);

			return new SuccessDataResult<User>(userExist.Data, userExist.Message);
		}

		private IDataResult<int> GetCompanyIdByUser(int userId)
		{
			var company = _userCompanyService.GetCompanyByUserId(userId);
			if (!company.Success) return new ErrorDataResult<int>(company.Message);

			return new SuccessDataResult<int>(company.Data.Id);
		}
		
		public IDataResult<User> GetByMailConfirmValue(string value)
		{
			var userExist = _userService.GetByMailConfirmValue(value);
			if (!userExist.Success) return new ErrorDataResult<User>(userExist.Message);

			return new SuccessDataResult<User>(userExist.Data);
		}

		public async Task<IResult> UpdateAsync(User user)
		{
			var result =  await _userService.UpdateAsync(user);
			if(!result.Success) return new ErrorResult(result.Message);

			return new SuccessResult(Messages.UserMailConfirmSuccessful);
		}

		void SendConfirmEmailWithTemplate(User user, int companyId)
		{
			string subject = "User Confirmation Email";
			string body = "Click on the link below to complete your registration";
			string link = "https://localhost:44358/api/Auth/confirmuser?value=" + user.MailConfirmValue; 
			string linkDescription = "Click here";

			var mailTemplate = _mailTemplateService.GetByTemplateName("Register", 2);//burayı gözden geçir company id 0 olmalı

			string templateBody = mailTemplate.Data.Value;
			templateBody = templateBody.Replace("{{title}}", subject);
			templateBody = templateBody.Replace("{{message}}", body);
			templateBody = templateBody.Replace("{{link}}", link);
			templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);

			var mailParameter = _mailParameterService.Get(companyId); 

			SendMailDto sendMailDto = new SendMailDto 
			{
				MailParameter = mailParameter.Data,
				Email = user.Email,
				Subject = subject,
				Body = templateBody
			};

			_mailService.SendMail(sendMailDto); 

			user.MailConfirmDate = DateTime.Now;
			_userService.UpdateAsync(user); 
		}



	}
}
