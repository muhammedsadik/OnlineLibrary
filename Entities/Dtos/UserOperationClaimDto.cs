﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public class UserOperationClaimDto
	{
		public int UserId { get; set; }
		public int CompanyId { get; set; }
		public int[] OperationClaimIds { get; set; }
	}
}
