using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public class AuthorBooksDto
	{
		public int AuthorId { get; set; }
		public int[] BookId { get; set; }
	}
}
