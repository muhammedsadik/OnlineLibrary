﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public class BookCategoryUpdateDto
	{
		public int Id { get; set; }
		public int CategoryId { get; set; }
		public int BookId { get; set; }
	}
}
