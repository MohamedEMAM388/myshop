using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.DTOS.ShoppingCart
{
    public class CartDto
    {
        public List<CartItemDTO> Items { get; set; } = [];

        public decimal OrderTotal {

            get {

                return Items.Sum(x => x.GetTotal()); 

            }
                
                }
    }
}
