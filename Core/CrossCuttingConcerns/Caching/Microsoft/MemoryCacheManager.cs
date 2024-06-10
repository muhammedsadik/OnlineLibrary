using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
	public class MemoryCacheManager : ICacheManager
	{
		private IMemoryCache _cache;

		public MemoryCacheManager()
		{
			_cache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
		}

		public void Add(string key, object data, int duration)
		{
			_cache.Set(key, data, TimeSpan.FromMinutes(duration));
		}

		public T Get<T>(string key)
		{
			return _cache.Get<T>(key);
		}

		public object Get(string key)
		{
			return _cache.Get(key);
		}

		public bool IsAdd(string key)
		{
			return _cache.TryGetValue(key, out _);
		}

		public void Remove(string key)
		{
			_cache.Remove(key);
		}

		public void RemoveByPattern(string pattern)
		{
			var coherentState = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);

			var coherentStateValue = coherentState.GetValue(_cache);

			var entriesCollection = coherentStateValue.GetType().GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

			var entriesCollectionValue = entriesCollection.GetValue(coherentStateValue) as ICollection;

			var keys = new List<string>();

			if (entriesCollectionValue != null)
			{
				foreach (var item in entriesCollectionValue)
				{
					var methodInfo = item.GetType().GetProperty("Key");

					var cacheCollectionValues = methodInfo.GetValue(item);

					keys.Add(cacheCollectionValues.ToString());
				}
			}

			foreach (var key in keys)
			{
				_cache.Remove(key);
			}
		}
	}



}

