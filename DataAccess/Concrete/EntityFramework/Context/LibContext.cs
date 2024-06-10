using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccess.Concrete.EntityFramework.Context
{
	public class LibContext : DbContext
	{

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("server=.\\sqlexpress;Initial Catalog=OnlineLibrary;TrustServerCertificate=True;Integrated Security=true;");
		}



		public DbSet<User> Users { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<AuthorBook> AuthorBooks { get; set; }
		public DbSet<BookCategory> BookCategories { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<UserCompany> UserCompanies { get; set; }
		public DbSet<OperationClaim> OperationClaims { get; set; }
		public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
		public DbSet<MailParameter> MailParameters { get; set; }
		public DbSet<MailTemplate> MailTemplates { get; set; }



		



	}
}
