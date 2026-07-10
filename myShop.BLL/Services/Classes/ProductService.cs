using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using myshop.Entities.Models;
using myshop.Entities.ViewModels;
using myshop.Models;
using myShop.BLL.Services.Interfaces;
using myShop.BLL.DTOS;
using myShop.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork , 
               IWebHostEnvironment webHostEnvironment , IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(ProductVM productVM, IFormFile file)
        {
            string? imagePath = null;

            try
            {
                string rootPath = _webHostEnvironment.WebRootPath;

                if (file is not null)
                {
                    string fileName = Guid.NewGuid().ToString();

                    var path = Path.Combine(rootPath, "Images", "Products");

                    var ext = Path.GetExtension(file.FileName);

                    imagePath = Path.Combine(path, fileName + ext);

                    await using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await file.CopyToAsync(fileStream);

                    productVM.Product.Img = @"Images\Products\" + fileName + ext;
                }

                await _unitOfWork.GetGenericRepository<myshop.Models.Product>()
                                 .AddAsync(productVM.Product);

                var isCreated = await _unitOfWork.SaveChangesAsync() > 0;

                if (!isCreated)
                {
                    DeleteImageIfExists(imagePath);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DeleteImageIfExists(imagePath);
                return false;
            }
        }
 
        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetGenericRepository<myshop.Models.Product>();
            var product = await repo.GetByIdAsync(id);
            if (product is null)
                return false;

            if (!string.IsNullOrEmpty(product.Img)) {
                var oldimg = Path.Combine(_webHostEnvironment.WebRootPath, product.Img.TrimStart('\\'));
                if (File.Exists(oldimg))
                        File.Delete(oldimg);

              
            }
            repo.Delete(product);
            return  await _unitOfWork.SaveChangesAsync() > 0;

        } // done

        public async Task<IEnumerable<ProductListDTO>> GetAllAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithLoadedDataAsync();
            if (!products.Any())  return [];
            // map from product to productVM
            return _mapper.Map<IEnumerable<ProductListDTO>>(products);
        } // done

        public async Task<IEnumerable<SelectListItem>> GetCategoriesAsync()
        {
            var Categories = await _unitOfWork.GetGenericRepository<Category>().GetAllAsync();

            return Categories.Select(x => new SelectListItem {
                      Text = x.Name, 
                      Value = x.Id.ToString() 
            });


        } // done

        public async Task<bool> UpdateAsync(ProductVM productVM, IFormFile? file)
        {

            // get root path 
            var rootPath = _webHostEnvironment.WebRootPath;
            // check if the client upload new image or not 
            if (file is not null)
            {
                // create a unique filename for the uploaded image
                string filename = Guid.NewGuid().ToString();
                // combine the root path with the folder path to save the image
                var Upload = Path.Combine(rootPath, @"Images\Products");
                // get the file extension of the uploaded image
                var ext = Path.GetExtension(file.FileName);

                // delete the old image file if it exists
                if (productVM.Product.Img != null)
                {
                    var oldimg = Path.Combine(rootPath, productVM.Product.Img.TrimStart('\\'));

                    if (File.Exists(oldimg))
                    {
                        File.Delete(oldimg);
                    }
                }

                // save the new image file to the server
                using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                {
                    await file.CopyToAsync(filestream);
                }
                // update the product's image path in the database
                productVM.Product.Img = @"Images\Products\" + filename + ext;
            }


            _unitOfWork.GetGenericRepository<Product>().Update(productVM.Product);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.GetGenericRepository<Product>().GetByIdAsync(id);
            // check if product is null
            if (product is null)
                return null;
            //  map from product to productVM
            return product; 
                
        } // done


        #region Helper Methods

        private static void DeleteImageIfExists(string? imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

        } // done



        #endregion
    }
}
