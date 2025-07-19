using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Helper
{
    public class PaginationResponse<TEntity>
    {
        public PaginationResponse(int pageSize, int pageIndex, int count, IEnumerable<TEntity> data,int? totalCount=4)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
            TotalCount = totalCount;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public int ? TotalCount { get; set; }
        public IEnumerable<TEntity> Data { get; set; }
    }
}