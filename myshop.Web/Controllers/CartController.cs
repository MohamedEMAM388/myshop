using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.BLL.DTOS.ShoppingCart;
using myShop.BLL.Services.Interfaces;

namespace myshop.PL.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService) 
        {
            _cartService = cartService;
        }
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productid) {

             await _cartService.Additem(productid);

            return RedirectToAction("Index", "Customer");
        }

        [HttpPost]
        public IActionResult RemoveItem(int productid) {

            _cartService.RemoveItem(productid);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult IncreaseQuantity(int productId)
        {
            _cartService.IncreaseQuantity(productId);

            return RedirectToAction(nameof(Index));
        }

       
        [HttpPost]
        public IActionResult DecreaseQuantity(int productId)
        {
            _cartService.DecreaseQuantity(productId);

            return RedirectToAction(nameof(Index));
        }

     
        [HttpPost]
        public IActionResult ClearCart()
        {
            _cartService.ClearCart();

            return RedirectToAction(nameof(Index));
        }
    }
}
