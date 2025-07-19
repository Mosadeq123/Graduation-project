using Store.G04.Core.Entities;

public class RawMaterial : BaseEntity<int>
{
    public string NameMaterial { get; set; }
    public string Description { get; set; }
    public string Quantity { get; set; }
    public string StitchLength { get; set; }
    public string YarnType { get; set; }
    public string PictureUrl { get; set; }

    // Foreign key for Machine
    public int? MachineId { get; set; }
    public MachineEntity Machine { get; set; } // Navigation property

    // Property for price
    public decimal UnitPrice { get; set; } // Represents the price per unit of the material
}