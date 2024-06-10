using Business.Constans;
using Core.Entities.Concrete;
using Entities.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
	public class UserValidator : AbstractValidator<UserDto>
	{
		public UserValidator()
		{
			RuleFor(x => x.FirstName).NotEmpty().WithMessage(Messages.NotEmpty);
			RuleFor(x => x.FirstName).Length(2, 30).WithMessage(Messages.LengthBetweenTwoAndThirty);
			RuleFor(x => x.LastName).NotEmpty().WithMessage(Messages.NotEmpty);
			RuleFor(x => x.LastName).Length(2, 30).WithMessage(Messages.LengthBetweenTwoAndThirty);
			RuleFor(x => x.Email).EmailAddress().WithMessage(Messages.EmailFormat);


		}
	}
}
