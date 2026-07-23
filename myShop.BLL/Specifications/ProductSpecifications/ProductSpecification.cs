using myshop.Models;
using myShop.BLL.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.Specifications.ProductSpecifications
{
    public class ProductSPecification : BaseSpecification<Product , int>
    {

        public ProductSPecification(ProductQueryParams queryParams) 
        {
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

    }
}
