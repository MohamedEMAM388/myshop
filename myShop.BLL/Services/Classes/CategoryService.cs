using AutoMapper;
using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myShop.BLL.Services.Interfaces;
using myShop.BLL.DTOS;
using myShop.DAL.Repositories.Classes;
using myShop.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace myShop.BLL.Services.Classes
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;

        private const string CategoriesCacheKey = "Categories";
        public CategoryService(IMapper mapper , IUnitOfWork unitOfWork ,
                       IMemoryCache memoryCache ) 
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }
        public async Task<bool> CreateAsync(CategoryDTO categoryVm)
        {
            if(categoryVm is null)
                return false;
            // map from categoryvm to category
            var category = _mapper.Map<Category>(categoryVm);
            await  _unitOfWork.GetGenericRepository<Category , int>().AddAsync(category);
            
            var result = await _unitOfWork.SaveChangesAsync() > 0;

            if (result)
                _memoryCache.Remove(CategoriesCacheKey);
            return result;

    
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            if (_memoryCache.TryGetValue(CategoriesCacheKey, out IEnumerable<CategoryDTO>? categories))
                  return categories!; 

            var categoryEntities = await _unitOfWork
                .GetGenericRepository<Category, int>()
                .GetAllAsync();

            categories = _mapper.Map<IEnumerable<CategoryDTO>>(categoryEntities);

            _memoryCache.Set(
                 CategoriesCacheKey,
                 categories,
                TimeSpan.FromMinutes(30));

            return categories;
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id)
        {
                
            var category = await _unitOfWork.GetGenericRepository<Category , int>().GetByIdAsync(id);
            if (category is null)
                return null;
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<bool> UpdateAsync(CategoryDTO categoryVm)
        {
            if (categoryVm is null)
                return false;
            var category = await _unitOfWork
                .GetGenericRepository<Category , int>()
                .GetByIdAsync(categoryVm.Id);

            if (category is null)
                return false;

            _mapper.Map(categoryVm, category);

            var result = await _unitOfWork.SaveChangesAsync() > 0;
            if (result)
                _memoryCache.Remove(CategoriesCacheKey);
            return result;
        }



        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _unitOfWork.GetGenericRepository<Category , int>().GetByIdAsync(id);
            if (category is null)
                return false;
            _unitOfWork.GetGenericRepository<Category , int>().Delete(category);
            var result = await _unitOfWork.SaveChangesAsync() > 0;
            if (result)
                _memoryCache.Remove(CategoriesCacheKey);
            return result;
        }
    }
}
