using Business.Constans;
using Entities.Concrete;
using Entities.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
	public class BookValidator : AbstractValidator<BookDto>
	{
		public BookValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(Messages.NotEmpty);
			RuleFor(x => x.Name).Length(2,30).WithMessage(Messages.LengthBetweenTwoAndThirty);
			RuleFor(x => x.Language).NotEmpty().WithMessage(Messages.NotEmpty);
			RuleFor(x => x.Price).GreaterThan(0).WithMessage(Messages.GreaterThanZero);
			RuleFor(x => x.Description).NotEmpty().WithMessage(Messages.NotEmpty);

		}
	}
}
