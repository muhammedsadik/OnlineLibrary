using Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
	public class EfGenericRepositoryBase<T, TContext> : IGenericRepository<T>
		where T : class, IEntity, new()
		where TContext : DbContext, new()
	{
		public async Task AddAsync(T entity)
		{
			using (var context = new TContext())
			{
				var AddedEntity = context.Entry(entity);
				AddedEntity.State = EntityState.Added;
				await context.SaveChangesAsync();
			}
		}

		public async Task<List<T>> GetAllAsync()
		{
			using (var context = new TContext())
			{
				return await context.Set<T>().ToListAsync();
			}
		}

		public async Task DeleteAsync(T entity)
		{
			using (var context = new TContext())
			{
				var DeletedEntity = context.Entry(entity);
				DeletedEntity.State = EntityState.Deleted;
				await context.SaveChangesAsync();
			}
		}

		public async Task UpdateAsync(T entity)
		{
			using (var context = new TContext())
			{
				var UpdateEntity = context.Entry(entity);
				UpdateEntity.State = EntityState.Modified;
				await context.SaveChangesAsync();
			}
		}

		public IList<T> Where(Expression<Func<T, bool>> filter = null)
		{
			using (var context = new TContext())
			{
				return filter == null ? context.Set<T>().ToList()
					: context.Set<T>().Where(filter).ToList();
			}
		}

		public T Get(Expression<Func<T, bool>> filter)
		{
			using (var context = new TContext())
			{
				return context.Set<T>().FirstOrDefault(filter);
			}
		}
		public bool IsExist(Expression<Func<T, bool>> filter)
		{
			using (var context = new TContext())
			{
				return context.Set<T>().Any(filter);
			}
		}

	}
}
