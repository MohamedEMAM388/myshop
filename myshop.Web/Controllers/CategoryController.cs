using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess;
using myshop.Entities.Models;
using myShop.BLL.Services.Interfaces;
using myShop.BLL.DTOS;
using System.Threading.Tasks;

namespace myshop.Web.Areas.Admin.Controllers
{


    [Authorize(Policy = "OnlyAdmin")]
    public class CategoryController : Controller
    {
      
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
                     _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        } // done 

        [HttpGet]
        
        public IActionResult Create()
        {

            return View();
        } // done 

        [HttpPost]

        public async Task<IActionResult> Create(CategoryDTO categoryVm)
        {
            if (!ModelState.IsValid)
                return View(categoryVm);

            var isCreated = await _categoryService.CreateAsync(categoryVm);

            if (!isCreated)
                return View(categoryVm);

            TempData["Create"] = "Item has Created Successfully";

            return RedirectToAction(nameof(Index));
        } // done 

        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null | id == 0)
            {
               return NotFound();
            }
            var categoryIndb = await _categoryService.GetByIdAsync(id!.Value);

            return View(categoryIndb);
        } // done 

        [HttpPost]
   
        public async Task<IActionResult> Edit(CategoryDTO categoryVm)
        {
            if(!ModelState.IsValid)
                return View(categoryVm);
            var isUpdated = await _categoryService.UpdateAsync(categoryVm);
            if(!isUpdated)
                return View(categoryVm);

            TempData["Update"] = "Data has Updated Successfully";
            return RedirectToAction("Index");

        } // done

        [HttpGet]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null | id == 0)
            {
              return NotFound();
            }
            var categoryIndb = await _categoryService.GetByIdAsync(id!.Value);
            if (categoryIndb is null)
                return NotFound();

            return View(categoryIndb);
        } // done

        [HttpPost]

        public async Task<IActionResult> DeleteCategory(int? id)
        {
            var isDeleted = await _categoryService.DeleteAsync(id!.Value);

            if (!isDeleted)
            {
                return NotFound();
            }

            TempData["Delete"] = "Item has Deleted Successfully";

            return RedirectToAction(nameof(Index));
        } // done 
    }
}
