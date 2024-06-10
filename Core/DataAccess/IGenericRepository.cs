using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
	public interface IGenericRepository<T> where T : class, IEntity, new()
	{
		Task<List<T>> GetAllAsync();
		IList<T> Where(Expression<Func<T,bool>> filter = null);
		bool IsExist(Expression<Func<T, bool>> filter);
		T Get(Expression<Func<T, bool>> filter);
		Task AddAsync(T entity);
		Task DeleteAsync(T entity);
		Task UpdateAsync(T entity);
	}
}
