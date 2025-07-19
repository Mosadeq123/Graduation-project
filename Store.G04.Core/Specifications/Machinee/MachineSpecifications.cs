using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Specifications.Machinee
{
    public class MachineSpecifications : BaseSpecifications<MachineEntity, int>
    {
        public MachineSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludeMachine();
        }
        //23
        //p.size=4
        //p.Index=1

        public MachineSpecifications(MachineeParams machineeSpec):base(
            P => (string.IsNullOrEmpty(machineeSpec.Search) || P.NameMachine.ToLower().Contains(machineeSpec.Search))
            )
        {
            ApplyIncludeMachine();

            //pageSize
            //pageIndex
            ApplyPagination(machineeSpec.pageSize * (machineeSpec.pageIndex - 1), machineeSpec.pageSize);
        }
        private void ApplyIncludeMachine()
        {
            Includes.Add(K => K.RawMaterials);
        }
    }
}
