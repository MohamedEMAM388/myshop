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

        public ProductSPecification(ProductQueryParams queryParams) : 
                                    base(p => string.IsNullOrWhiteSpace(queryParams.Search) || p.Name.Contains(queryParams.Search)) 
        {

            // pagination
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);

            // switch case for sorting
            switch (queryParams.Sort)
            {

                case ProductSorting.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSorting.PriceDesc:
                    AddOrderByDescinding(p => p.Price);
                    break;
                case ProductSorting.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSorting.NameDesc:
                    AddOrderByDescinding(p => p.Name);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }
        }

    }
}
