using Microsoft.AspNetCore.Http;

namespace Store.G04.Core.Dtos.RawMaterials;
public class MachineDtos
{
    public int Id { get; set; }
    public string NameMachine { get; set; }
    public string Description { get; set; }
    public string NeedlesCount { get; set; }
    public string MachineType { get; set; }
    public string Softness { get; set; }
    public string Width { get; set; }
    public string PictureUrl { get; set; }
    public DateTime CreateAt { get; set; }
}
public class MachineWithImageDto : MachineDtos
{
    public IFormFile? ImageFile { get; set; }
}