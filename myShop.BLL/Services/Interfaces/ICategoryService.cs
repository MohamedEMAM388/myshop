using myshop.Entities.Models;
using myShop.BLL.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();

        Task<CategoryDTO?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CategoryDTO categoryVm);

        Task<bool> UpdateAsync(CategoryDTO categoryVm);

        Task<bool> DeleteAsync(int id);
    }
}
