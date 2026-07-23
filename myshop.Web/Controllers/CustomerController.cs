using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.BLL.Services.Interfaces;

namespace myshop.PL.Controllers
{

    public class CustomerController : Controller
    {
        private readonly IProductService _productService;

        public CustomerController(IProductService productService) {
            _productService = productService;
        }
        [Route("Customer")]
        [Route("Customer/Index")]
        public async Task<IActionResult> Index(int page = 1)
        {

            const int pageSize = 8;
            var products = await _productService.GetPagedAsync(page , pageSize);
            return View(products);
        }
    }
}
