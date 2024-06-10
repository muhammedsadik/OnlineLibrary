using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
	public class AutofacBusinessModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<AuthManager>().As<IAuthService>();
			builder.RegisterType<JwtTokenHelper>().As<ITokenHelper>();

			builder.RegisterType<AuthorBookManager>().As<IAuthorBookService>();
			builder.RegisterType<AuthorBookDal>().As<IAuthorBookDal>();

			builder.RegisterType<AuthorManager>().As<IAuthorService>();
			builder.RegisterType<AuthorDal>().As<IAuthorDal>();

			builder.RegisterType<CategoryBookManager>().As<ICategoryBookService>();
			builder.RegisterType<CategoryBookDal>().As<ICategoryBookDal>();

			builder.RegisterType<BookManager>().As<IBookService>();
			builder.RegisterType<BookDal>().As<IBookDal>();

			builder.RegisterType<CategoryManager>().As<ICategoryService>();
			builder.RegisterType<CategoryDal>().As<ICategoryDal>();

			builder.RegisterType<CompanyManager>().As<ICompanyService>();
			builder.RegisterType<CompanyDal>().As<ICompanyDal>();

			builder.RegisterType<OperationClaimManager>().As<IOperationClaimService>();
			builder.RegisterType<OperationClaimDal>().As<IOperationClaimDal>();

			builder.RegisterType<UserCompanyManager>().As<IUserCompanyService>();
			builder.RegisterType<UserCompanyDal>().As<IUserCompanyDal>();

			builder.RegisterType<UserOperationClaimManager>().As<IUserOperationClaimService>();
			builder.RegisterType<UserOperationClaimDal>().As<IUserOperationClaimDal>();

			builder.RegisterType<UserManager>().As<IUserService>();
			builder.RegisterType<UserDal>().As<IUserDal>();
			
			builder.RegisterType<MailParameterManager>().As<IMailParameterService>();
			builder.RegisterType<MailParameterDal>().As<IMailParameterDal>();
			
			builder.RegisterType<MailManager>().As<IMailService>();
			builder.RegisterType<MailDal>().As<IMailDal>();
			
			builder.RegisterType<MailTemplateManager>().As<IMailTemplateService>();
			builder.RegisterType<MailTemplateDal>().As<IMailTemplateDal>();

			var essembly = System.Reflection.Assembly.GetExecutingAssembly();	

			builder.RegisterAssemblyTypes(essembly).AsImplementedInterfaces().EnableInterfaceInterceptors(new ProxyGenerationOptions()
			{
				Selector = new AspectInterceptorSelector()

			}).SingleInstance();

		}

	}
}
