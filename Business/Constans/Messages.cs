using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constans 
{
  public class Messages
	{
		//Comman Usage
		public static string NotEmpty = "This field cannot be empty";
		public static string LengthBetweenTwoAndThirty = "The length must be between 2 and 30";
		public static string EmailFormat = "Please enter in E-mail format";
		public static string GreaterThanZero = "Please enter greater than zero";
		public static string AuthorizationDenied = "you don't have authority";



		//Registert
		public static string SuccessfulLogin = "Successful login";
    public static string PasswordError = "Password error";
    public static string UserRegistered = "The new user registered";
		public static string NotAuthYet = "Not Auth yet";


		//User
		public static string UserAdded = "The new user added";
    public static string UserDeleted = "The user deleted";
    public static string UserUpdated = "The user updated";
		public static string UserNotFound = "User not found";
		public static string UserAlreadyExists = "The user already exists";
		


		//Author
		public static string AuthorAdded = "The new author added";
    public static string AuthorDeleted = "The author deleted";
    public static string AuthorUpdated = "The author updated";
    public static string AuthorNotFound = "The author not found";
    public static string AuthorAlreadyExists = "The author already exists";

		//Book
		public static string BookAdded = "The new book added";
		public static string BookDeleted = "The book deleted";
		public static string BookUpdated = "The book updated";
		public static string BookNotFound = "The book not found";
		public static string BookAlreadyExists = "The book already exists";


		//Category
		public static string CategoryAdded = "The new category added";
		public static string CategoryDeleted = "The category deleted";
		public static string CategoryUpdated = "The category updated";
		public static string CategoryNotFound = "The category not found";
		public static string CategoryAlreadyExists = "The category already exists";
    
		//Company
		public static string CompanyAdded = "The new company added";
		public static string CompanyDeleted = "The company deleted";
		public static string CompanyUpdated = "The company updated";
		public static string CompanyNotFound = "The company not found";
		public static string CompanyAlreadyExists = "The company already exists";


				//Rule
    public static string CompanyLength = "Company name must consist of at least 3 characters";


		//OperationClaim
		public static string OperationClaimAdded = "The new operation claim added";
		public static string OperationClaimDeleted = "The operation claim deleted";
		public static string OperationClaimUpdated = "The operation claim updated";
		public static string OperationClaimNotFound = "The operation claim not found";
		public static string OperationClaimAlreadyExists = "The operation claim already exists";

		//AuthorBook
		public static string AuthorBookAdded = "The new AuthorBook added";
		public static string AuthorBookDeleted = "The AuthorBook deleted";
		public static string AuthorBookUpdated = "The AuthorBook updated";
		public static string AuthorBookNotFound = "The AuthorBook not found";
		public static string AuthorBookAlreadyExists = "The AuthorBook already exists";

		//BookCategory
		public static string BookCategoryAdded = "The new BookCategory added";
		public static string BookCategoryDeleted = "The BookCategory deleted";
		public static string BookCategoryUpdated = "The BookCategory updated";
		public static string BookCategoryNotFound = "The BookCategory not found";
		public static string BookCategoryListNotFound = "The BookCategory list not found";
		public static string BookCategoryAlreadyExists = "The BookCategory already exists";
		
		//UserCompany
		public static string UserCompanyAdded = "The new UserCompany added";
		public static string UserCompanyDeleted = "The UserCompany deleted";
		public static string UserCompanyUpdated = "The UserCompany updated";
		public static string UserCompanyNotFound = "The UserCompany not found";
		public static string UserCompanyListNotFound = "The UserCompany list not found";
		public static string UserCompanyAlreadyExists = "The UserCompany already exists";

		//UserOperationClaim
		public static string UserOperationClaimAdded = "The new UserOperationClaim added";
		public static string UserOperationClaimDeleted = "The UserOperationClaim deleted";
		public static string UserOperationClaimUpdated = "The UserOperationClaim updated";
		public static string UserOperationClaimNotFound = "The UserOperationClaim not found";
		public static string UserOperationClaimListNotFound = "The UserOperationClaim list not found";
		public static string UserOperationClaimAlreadyExists = "The UserOperationClaim already exists";


		//Email
		public static string MailConfirmValueNotFound = "Mail confirm value not found";
		public static string MailParameterUpdated = "Mail parameter updated";
		public static string MailParameterNotFound = "Mail parameter not found";
		public static string MailSendSuccessful = "Email was sent successfully";
		public static string MailTemplateUpdated = "Email template Updated successfully";
		public static string MailTemplateDeleted = "Email template Deleted successfully";
		public static string MailTemplateAdded = "Email template saved successfully";
		public static string MailTemplateNotFound = "Email template not found";
		public static string UserMailConfirmSuccessful = "User mail confirm successfully";
		public static string MailConfirmSendSuccessful = "Email sent successfully";
		public static string MailAlreadyConfirm = "Email already confirm";
		public static string MailConfirmTimeHasNotExpired = "Email confirm time (5 minute) has not expired";




	}
}
