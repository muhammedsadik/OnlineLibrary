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
	public class CategoryValidator : AbstractValidator<CategoryDto>
	{
		public CategoryValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(Messages.NotEmpty);
			RuleFor(x => x.Name).Length(2, 30).WithMessage(Messages.LengthBetweenTwoAndThirty);
		}
	}
}
