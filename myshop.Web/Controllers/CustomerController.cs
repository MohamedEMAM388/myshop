using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.BLL.QueryParams;
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
        public async Task<IActionResult> Index(ProductQueryParams queryParams)
        {
            var products = await _productService.GetPagedAsync(queryParams);
            return View(products);
        }
    }
}
