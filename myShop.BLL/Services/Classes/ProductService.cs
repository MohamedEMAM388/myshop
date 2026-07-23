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
using myShop.BLL.Services.AttachmentServiec;
using myShop.BLL.Specifications.ProductSpecifications;
using myShop.BLL.QueryParams;

namespace myShop.BLL.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IAttachmentServiec _attachmentServiec;

        public ProductService(IUnitOfWork unitOfWork , 
               IWebHostEnvironment webHostEnvironment , IMapper mapper ,
               IAttachmentServiec attachmentServiec) 
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _attachmentServiec = attachmentServiec;
        }
        public async Task<bool> CreateAsync(ProductVM productVM)
        {


            try
            {
                string rootPath = _webHostEnvironment.WebRootPath;
                if (productVM.ImageFile is not null)
                { 

                    var imagePath = await _attachmentServiec.UploadAsync(
                        productVM.ImageFile.OpenReadStream(),
                        productVM.ImageFile.FileName,
                        "Products");

                    if(string.IsNullOrWhiteSpace(imagePath))
                        return false;

                    productVM.Product.Img = imagePath;
                }

                await _unitOfWork.GetGenericRepository<Product , int>()
                                 .AddAsync(productVM.Product);

                var isCreated = await _unitOfWork.SaveChangesAsync() > 0;

                if (!isCreated)
                {
                    _attachmentServiec.Delete(productVM.ImageFile!.FileName, "Products");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _attachmentServiec.Delete(productVM.ImageFile!.FileName, "Products");
                return false;
            }
        }
 
        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetGenericRepository<Product , int>();
            var product = await repo.GetByIdAsync(id);
            if (product is null)
                return false;

            if (!string.IsNullOrEmpty(product.Img)) {

                var imageName = Path.GetFileName(product.Img);

                _attachmentServiec.Delete(imageName, "uploads/Products");

              
            }
            repo.Delete(product);
            return  await _unitOfWork.SaveChangesAsync() > 0;

        } // done

        public async Task<PaginatedList<ProductListDTO>> GetPagedAsync(ProductQueryParams queryParams)
        {
            var spec = new ProductSPecification(queryParams);

            var products = await _unitOfWork.ProductRepository
                .GetAllWithSpecification(spec);

            var totalCount = await _unitOfWork
                .GetGenericRepository<Product, int>()
                .CountAsync();

            var productsDTO = _mapper.Map<List<ProductListDTO>>(products);

            return new PaginatedList<ProductListDTO>(
                productsDTO,
                totalCount,
                queryParams.PageIndex,
                queryParams.PageSize
            );


        } // done

        public async Task<IEnumerable<SelectListItem>> GetCategoriesAsync()
        {
            var Categories = await _unitOfWork.GetGenericRepository<Category , int>().GetAllAsync();

            return Categories.Select(x => new SelectListItem {
                      Text = x.Name, 
                      Value = x.Id.ToString() 
            });


        } // done

        public async Task<bool> UpdateAsync(ProductVM productVM)
        {
            var repo = _unitOfWork.GetGenericRepository<Product , int>();

            var product = await repo.GetByIdAsync(productVM.Product.Id);

            if (product is null)
                return false;

            _mapper.Map(productVM.Product, product);

            string? oldImg = null;
            string? newImagePath = null;

            if (productVM.ImageFile is not null)
            {
                newImagePath = await _attachmentServiec.UploadAsync(
                    productVM.ImageFile.OpenReadStream(),
                    productVM.ImageFile.FileName,
                    "Products");

                if (string.IsNullOrWhiteSpace(newImagePath))
                    return false;

                oldImg = product.Img;
                product.Img = newImagePath;
            }

            repo.Update(product);

            var result = await _unitOfWork.SaveChangesAsync() > 0;

            if (result && !string.IsNullOrWhiteSpace(oldImg))
            {
                var oldImageName = Path.GetFileName(oldImg);
                _attachmentServiec.Delete(oldImageName, "uploads/Products");
            }

            if (!result && !string.IsNullOrWhiteSpace(newImagePath))
            {
                // الحفظ فشل بعد رفع صورة جديدة → امسحها عشان متفضلش orphaned
                var newImageName = Path.GetFileName(newImagePath);
                _attachmentServiec.Delete(newImageName, "uploads/Products");
            }

            return result;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.GetGenericRepository<Product , int>().GetByIdAsync(id);
            // check if product is null
            if (product is null)
                return null;
            //  map from product to productVM
            return product; 
                
        } // done

        public async Task<IEnumerable<ProductListDTO>> GetAllAsync()
        {
            var products =   _unitOfWork.ProductRepository.GetAllWithLoadedData();
            return _mapper.Map<IEnumerable<ProductListDTO>>(products);

        }
    }
}
