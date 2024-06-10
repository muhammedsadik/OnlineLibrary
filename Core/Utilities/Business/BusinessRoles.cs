using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
	public class BusinessRoles
	{
		public static IResult Run(params IResult[] logic)
		{
			foreach (var result in logic)
			{
				if (!result.Success)
					return result;
			}

			return null;
		}
	}
}
