using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.G04.Core.Dtos.RawMaterials;
 
namespace Store.G04.Core.Mapping;
public class MachineUrlResolver : IValueResolver<MachineEntity, MachineDtos, string>
{
    private readonly IConfiguration _configuration;

    public MachineUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Resolve(MachineEntity source, MachineDtos destination, string destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.PictureUrl))
        {
            return string.Empty; 
        }

        return $"{_configuration["BaseURL"]}{source.PictureUrl}";
    }
}