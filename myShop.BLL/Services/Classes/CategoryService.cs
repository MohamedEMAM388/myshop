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

namespace myShop.BLL.Services.Classes
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IMapper mapper , IUnitOfWork unitOfWork) 
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateAsync(CategoryDTO categoryVm)
        {
            if(categoryVm is null)
                return false;
            // map from categoryvm to category
            var category = _mapper.Map<Category>(categoryVm);
            await  _unitOfWork.GetGenericRepository<Category , int>().AddAsync(category);
            return await _unitOfWork.SaveChangesAsync() > 0;

    
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var categories = await _unitOfWork.GetGenericRepository<Category , int>().GetAllAsync();
            // map from category to categoryvm
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
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
            var category = await _unitOfWork
                .GetGenericRepository<Category , int>()
                .GetByIdAsync(categoryVm.Id);

            if (category is null)
                return false;

            _mapper.Map(categoryVm, category);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }



        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _unitOfWork.GetGenericRepository<Category , int>().GetByIdAsync(id);
            if (category is null)
                return false;
            _unitOfWork.GetGenericRepository<Category , int>().Delete(category);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
