using Store.G04.Core.Entities;
public class MachineEntity : BaseEntity<int>
{
    public string NameMachine { get; set; }
    public string Description { get; set; }
    public string NeedlesCount { get; set; }
    public string MachineType { get; set; }
    public string Softness { get; set; }
    public string Width { get; set; }
    public string PictureUrl { get; set; } // خاصية PictureUrl

    // Collection of RawMaterials
    public ICollection<RawMaterial> RawMaterials { get; set; }
}