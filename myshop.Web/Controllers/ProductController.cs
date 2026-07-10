using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using myshop.DataAccess;
using myshop.Entities.Models;
using myshop.Entities.ViewModels;
using myshop.Models;
using myShop.BLL.Services.Interfaces;
using System.Threading.Tasks;

namespace myshop.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        private readonly IProductService _productService;

        public ProductController(
               IProductService productService)
        {
           
          
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }   // done

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var products = await _productService.GetAllAsync();

            return Json(new { data = products });
        } // done

        [HttpGet]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Create()
        {
            ProductVM productVM = new()
            {
                Product = new Product(),
                CategoryList = await _productService.GetCategoriesAsync()
            };
            return View(productVM);
        } // done 

        [HttpPost]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Create(ProductVM productVM,IFormFile file)
        {

            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return the view with
                // the productVM and category list
                productVM.CategoryList = await _productService.GetCategoriesAsync();
                return View(productVM);
            }
            var result = await _productService.CreateAsync(productVM, file);

            if (!result)
            {
                TempData["Error"] = "Error while creating product.";
                productVM.CategoryList = await _productService.GetCategoriesAsync();
                return View(productVM);
            }

            TempData["Create"] = "Product has been created successfully.";
            return RedirectToAction(nameof(Index));

          
        } // done

        [HttpGet]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = await _productService.GetByIdAsync(id.Value);

            if (product == null)
            {
                return NotFound();
            }

            ProductVM productVM = new ProductVM()
            {
                Product = product,
                CategoryList = await _productService.GetCategoriesAsync()
            };

            return View(productVM);
        } // done
        
        [HttpPost]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Edit(ProductVM productVM, IFormFile? file)
        {
            if (!ModelState.IsValid) { 
            
                productVM.CategoryList = await _productService.GetCategoriesAsync();
                return View(productVM);
            }

            // update the product 
            var isUpdated = await _productService.UpdateAsync(productVM, file);
            if (!isUpdated) {
                productVM.CategoryList = await _productService.GetCategoriesAsync();
                return View(productVM);

            }
            TempData["Update"] = "Data has Updated Successfully";
                return RedirectToAction("Index");
        }
        
        [HttpDelete]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Delete(int id)
        {

           var isDeleted = await _productService.DeleteAsync(id);

            if (!isDeleted)
                return Json(new { success = false, message = "Error while Deleting" });

            return Json(new { success = true, message = "Product has been deleted successfully" });
        } // done


    }
}
