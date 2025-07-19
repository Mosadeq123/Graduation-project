using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Specifications.RawMaterialss
{
    public class RawMaterialSpecifications : BaseSpecifications<RawMaterial,int>
    {
        public RawMaterialSpecifications(int id) :base(P => P.Id == id)
        {
            ApplyInclude();
        }
        public RawMaterialSpecifications(RawMaterialssSpecParams rawMaterialssSpec) :base(
            
            P=>(string.IsNullOrEmpty(rawMaterialssSpec.Search) || P.NameMaterial.ToLower().Contains(rawMaterialssSpec.Search))
            )

        {
            ApplyInclude();
            ApplyPagination(rawMaterialssSpec.pageSize * (rawMaterialssSpec.pageIndex - 1), rawMaterialssSpec.pageSize);
        }

        private void ApplyInclude()
        {
            Includes.Add(p => p.Machine);
        }
    }
}
