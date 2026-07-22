using myShop.BLL.DTOS.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.Services.Interfaces
{
    public interface ICartService
    {
        // get cart
        public CartDto GetCart();

        // add item
        public Task Additem(int productId);

        // remove item 
        public void RemoveItem(int productid);

        // increase Quantity 
        public void IncreaseQuantity(int productid);

        // decrease quantity
        public void DecreaseQuantity(int productid);

        // clear item 
        public void ClearCart();

    }
}
