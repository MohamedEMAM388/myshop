using Microsoft.AspNetCore.Http;
using myShop.BLL.DTOS.ShoppingCart;
using myShop.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace myShop.BLL.Services.Classes
{
    public class CartService : ICartService
    {
        // httpcontex have info for current http request  
        private readonly IHttpContextAccessor _httpContextAccessor;
        // key for store cart in session
        private const string CartSessionKey = "Cart";
        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void Additem(CartItemDTO cartItem)
        {
            // get cart 
            var cart = GetCart();

            // check if item is exist 
            var existingItem = cart.Items.
                               FirstOrDefault(x => x.ProductId == cartItem.ProductId);

            // if existingItem true then increase quantity
            if (existingItem is not null)
            {

                existingItem.Quantity += cartItem.Quantity;
            }
            else
            { 
            
                // add item to cart 
                cart.Items.Add(cartItem);

            }

            SaveCart(cart);

        } // done 

        public void ClearCart()
        {
            _httpContextAccessor.HttpContext.Session.Remove(CartSessionKey);
        } // done 

        public void DecreaseQuantity(int productid)
        {
            var cart = GetCart();

            var item = cart.Items
                       .FirstOrDefault(x => x.ProductId == productid);

            if (item is null)
                return;

            item.Quantity--;
            if (item.Quantity <= 0)
            {
                cart.Items.Remove(item);
            }

            SaveCart(cart);
        }

        public CartDto GetCart()
        {
            // define session  
            var session = _httpContextAccessor.HttpContext.Session;

            // get cart from json 
            var jsonCart = session.GetString(CartSessionKey);

            // check if jsoncart is null
            if (string.IsNullOrWhiteSpace(jsonCart))
                return new CartDto();

            // deserialiize from json to object
            return JsonSerializer.Deserialize<CartDto>(jsonCart);
        } // done 

        public void IncreaseQuantity(int productid)
        {
            var cart = GetCart();

            var item = cart.Items.
                       FirstOrDefault(x => x.ProductId == productid);

            if (item is null)
                return;

            item.Quantity++;
            SaveCart(cart);
        } // done 

        public void RemoveItem(int productid)
        {
            // get cart 
            var cart = GetCart();
            // check if item exist in cart 
            var existingItem = cart.Items.
                               FirstOrDefault(x => x.ProductId == productid);

            // check if item not null
            if(existingItem is not null)
                   cart.Items.Remove(existingItem);
            SaveCart(cart);

        } // done 

        #region Helper Method

        private void SaveCart(CartDto cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);

            _httpContextAccessor
                .HttpContext
                .Session
                .SetString(
                    CartSessionKey,
                    cartJson
                );
        }

        #endregion
    }
}
