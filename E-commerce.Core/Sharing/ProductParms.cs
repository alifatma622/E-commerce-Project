using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.Sharing
{
    public class ProductParms
    {
        //int? CategoryId, string? sort, int pageNumber, int pageSize
        public string? Sort { get; set; }
        public int? CategoryId { get; set; }

        private int _pageSize = 6; 
        private const int MaxPageSize = 10; //

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : (value <= 0 ? 6: value);
        }

        public int PageNumber { get; set; } = 1;

        public string? Search { get; set; }
    }
}
