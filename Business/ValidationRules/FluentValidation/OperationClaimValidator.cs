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
	public class OperationClaimValidator : AbstractValidator<OperationClaimDto>
	{
		public OperationClaimValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage(Messages.NotEmpty);
		}
	}
}
