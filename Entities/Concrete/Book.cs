﻿using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
	public class Book : IEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Language { get; set; }
		public decimal Price { get; set; }
		public string Description { get; set; }
		public DateTime AddedAt { get; set; }
		public bool IsActive { get; set; }
	}
}
