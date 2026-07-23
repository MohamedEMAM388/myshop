using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.Entities.ViewModels;
using myshop.Models;
using myShop.BLL.DTOS;
using myShop.BLL.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.Services.Interfaces
{
    public interface IProductService
    {
        // Get all products
        Task<PaginatedList<ProductListDTO>> GetPagedAsync(ProductQueryParams queryParams);

        Task<IEnumerable<ProductListDTO>> GetAllAsync();

        // Get all categories for dropdown
        Task <IEnumerable<SelectListItem>> GetCategoriesAsync();

        // Create a new product
        Task<bool> CreateAsync(ProductVM productVM);

        // get a product by id
        Task<Product?> GetByIdAsync(int id);

        // Update an existing product
        Task<bool> UpdateAsync(ProductVM productVM);

        // Delete a product by id
        Task<bool> DeleteAsync(int id);
    }
}
