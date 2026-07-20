using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.DTOS.ShoppingCart
{
    public class CartItemDTO
    {
                //        ProductId
                //ProductName
                //Price
                //Quantity
                //ImageUrl

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public decimal GetTotal() => Price * Quantity;
    }
}
