using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
	public class AuthorBook : IEntity
	{
		public int Id { get; set; }
		public int AuthorId { get; set; }
		public int BookId { get; set; }
		public DateTime AddedAt { get; set; }
	}
}
