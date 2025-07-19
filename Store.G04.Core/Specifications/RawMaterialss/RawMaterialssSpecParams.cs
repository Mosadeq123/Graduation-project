using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Specifications.RawMaterialss
{
    public class RawMaterialssSpecParams
    {
        private string? search;
            public string? Search
            {
                get {  return search; }
                set { search = value?.ToLower(); }
            }
        public int pageSize { get; set; } = 4;
        public int pageIndex { get; set; } = 1;
    }
}
