using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.BLL.QueryParams
{
    public class ProductQueryParams
    {

        #region Paginattion
        private int _pageIndex = 1;
        public int PageIndex
        {

            get => _pageIndex;

            set
            {

                _pageIndex = (value <= 0) ? _pageIndex : value;
            }
        }

        private const int MaxPageSize = 10;
        private const int DefaultPageSize = 8;
        private int _pagesize = DefaultPageSize;

        public int PageSize
        {
            get => _pagesize;

            set
            {

                if (value <= 0)
                    _pagesize = DefaultPageSize;
                else if (value > MaxPageSize)
                    _pagesize = MaxPageSize;
                else
                    _pagesize = value;
            }
        }
        #endregion

        public string? Search { get; set; }

        public ProductSorting? Sort { get; set; }

    }
}
