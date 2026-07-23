using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using myshop.Models;
using myShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
    public class OrderDetail : BaseEntity<int>
    {
       

        public int OrderHeaderId { get; set; }
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }

        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }


    }
}
