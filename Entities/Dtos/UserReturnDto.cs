using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public class UserReturnDto
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public DateTime AddedAt { get; set; }
		public bool IsActive { get; set; }
		public bool IsMailConfirm { get; set; }
	}
}
