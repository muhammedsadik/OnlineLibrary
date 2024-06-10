﻿using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
	public class MailParameter : IEntity
	{
		public int Id { get; set; }
		public int CompanyId { get; set; }
		public string Email { get; set; }
		public string EmailPassword { get; set; }
		public string SMTP { get; set; }
		public int Port { get; set; }
		public bool SSL { get; set; }
	}
}