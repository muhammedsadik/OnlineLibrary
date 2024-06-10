using Business.Abstract;
using Business.BusinessAspect;
using Business.Constans;
using Business.ValidationRules.FluentValidation;
using Core.Aspect.Autofac.Caching;
using Core.Aspect.Autofac.Validation;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class CategoryManager : ICategoryService
	{
		private readonly ICategoryDal _categoryDal;

		public CategoryManager(ICategoryDal categoryDal)
		{
			_categoryDal = categoryDal;
		}


		[SecuredOperation("Category.Create")]
		[CacheRemoveAspect(pattern: "ICategoryDal.Get")]
		[ValidationAspect(typeof(CategoryValidator))]
		public async Task<IResult> AddAsync(CategoryDto categoryDto)
		{
			var categoryExist = _categoryDal.IsExist(b => b.Name == categoryDto.Name);
			if (categoryExist) return await Task.FromResult<IResult>(new ErrorResult(Messages.CategoryAlreadyExists));

			var category = new Category
			{
				Name = categoryDto.Name,
				AddedAt = DateTime.Now
			};

			await _categoryDal.AddAsync(category);

			return new SuccessResult(Messages.CategoryAdded);
		}



		[SecuredOperation("Category.Delete")]
		[CacheRemoveAspect(pattern: "ICategoryDal.Get")]
		public async Task<IResult> DeleteAsync(int categoryId)
		{
			var categoryExist = GetById(categoryId);
			if (!categoryExist.Success) return new ErrorResult(Messages.CategoryNotFound);

			await _categoryDal.DeleteAsync(categoryExist.Data);

			return new SuccessResult(Messages.CategoryDeleted);
		}


		[SecuredOperation("Category.List")]
		[CacheAspect(duration: 10)]
		public async Task<IDataResult<List<Category>>> GetAllAsync()
		{
			var categoryExist = await _categoryDal.GetAllAsync();
			if (categoryExist == null) return new ErrorDataResult<List<Category>>(Messages.CategoryNotFound);

			return new SuccessDataResult<List<Category>>(categoryExist);
		}


		[SecuredOperation("Category.List")]
		public IDataResult<Category> GetById(int id)
		{
			var result = _categoryDal.Get(a => a.Id == id);
			if (result == null)
				return new ErrorDataResult<Category>(Messages.CategoryNotFound);

			return new SuccessDataResult<Category>(result);
		}
		

		[SecuredOperation("Category.Update")]
		[CacheRemoveAspect(pattern: "ICategoryDal.Get")]
		[ValidationAspect(typeof(CategoryValidator))]
		public async Task<IResult> UpdateAsync(CategoryDto categoryDto)
		{
			var categoryExist = _categoryDal.IsExist(b => b.Name == categoryDto.Name);
			if (categoryExist) return await Task.FromResult<IResult>(new ErrorResult(Messages.CategoryAlreadyExists));

			var category = GetById(categoryDto.Id).Data;
			if (category == null) return await Task.FromResult<IResult>(new ErrorResult(Messages.CategoryNotFound));

			category.Name = categoryDto.Name;
			category.AddedAt = DateTime.Now;

			await _categoryDal.UpdateAsync(category);

			return new SuccessResult(Messages.CategoryUpdated);
		}



	}
}
