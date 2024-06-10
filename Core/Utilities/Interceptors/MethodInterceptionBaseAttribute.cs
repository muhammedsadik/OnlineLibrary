using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Core.Utilities.Interceptors
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple =true)]
	public abstract class MethodInterceptionBaseAttribute : Attribute , IInterceptor
	{
		public int priority {  get; set; }

		public virtual void Intercept(IInvocation invocation)
		{
		}
	}
}
