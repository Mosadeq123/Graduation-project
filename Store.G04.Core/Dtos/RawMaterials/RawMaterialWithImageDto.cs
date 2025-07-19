using Microsoft.AspNetCore.Http;

namespace Store.G04.Core.Dtos.RawMaterials;
public class RawMaterialWithImageDto : RawMaterialDtos
{
    public IFormFile? ImageFile { get; set; } = null;
}
