using Business.Abstract;
using Business.Concrete;
using Business.Constans;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using LibraryAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.Test
{
	public class UserManagerTest
	{
		private readonly Mock<IUserDal> myMock;
		private readonly UserManager userManager;

		public UserManagerTest()
		{
			myMock = new Mock<IUserDal>();
			//userManager = new UserManager(new UserDal());
			userManager = new UserManager(myMock.Object);
		}
		[Theory]
		[InlineData(1)]
		public void GetById_ValidValue_ReturnUser(int id)
		{

			myMock.Setup(x => x.Get(u => u.Id == id)).Returns(new User { Id = 1, FirstName = "sadık" });

			var userResult = userManager.GetById(id);

			var result = Assert.IsType<User>(userResult.Data);

		}


		[Theory]
		[InlineData(1)]
		public void GetById_ValidValue_ReturnDataResult(int id)//test metodları geriye birşey dönmez
		{

			myMock.Setup(x => x.Get(u => u.Id == id)).Returns(new User { Id = 1, FirstName = "sadık" });

			var userResult = userManager.GetById(id);

			var result = Assert.IsType<SuccessDataResult<User>>(userResult);

		}

		[Theory]
		[InlineData(1)]
		public void GetById_UserIsInvalid_ReturnErrorMessage(int id)
		{
			var nullResult = myMock.Setup(x => x.Get(u => u.Id == id));

			var managerResult = userManager.GetById(id);

			var result = Assert.IsType<ErrorDataResult<User>>(managerResult);

			Assert.Equal(Messages.UserNotFound,result.Message);
		}














	}
}
