using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Dtos.RawMaterials
{
    public class RawMaterialDtos
    {
        public int Id { get; set; }
        public string NameMaterial { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string StitchLength { get; set; }
        public string YarnType { get; set; }
        public string? PictureUrl { get; set; }

        // Foreign key for Machine
        public int? MachineId { get; set; }
        public string? NameMachine { get; set; } // Navigation property
    }
}