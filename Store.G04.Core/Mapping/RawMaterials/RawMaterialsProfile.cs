using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.G04.Core.Dtos;
using Store.G04.Core.Dtos.RawMaterials;
using Store.G04.Core.Entities;

namespace Store.G04.Core.Mapping.RawMaterials;
public class RawMaterialsProfile : Profile
{
    public RawMaterialsProfile(IConfiguration configuration)
    {
        CreateMap<RawMaterial, RawMaterialDtos>()
            .ForMember(d => d.NameMachine, options => options.MapFrom(s => s.Machine.NameMachine))
            .ForMember(d => d.PictureUrl, options => options.MapFrom(new PictureUrlResolver(configuration)));

        CreateMap<RawMaterialDtos, RawMaterial>()
            .ForMember(dest => dest.Machine, opt => opt.Ignore());

        CreateMap<RawMaterial, RawMaterialWithImageDto>()
    .ForMember(d => d.NameMachine, options => options.MapFrom(s => s.Machine.NameMachine))
    .ForMember(d => d.PictureUrl, options => options.MapFrom(new PictureUrlResolver(configuration)));

        CreateMap<RawMaterialWithImageDto, RawMaterial>()
            .ForMember(dest => dest.Machine, opt => opt.Ignore());
        
        CreateMap<MachineEntity, MachineDtos>()
            .ForMember(d => d.PictureUrl, options => options.MapFrom(new MachineUrlResolver(configuration)));

        CreateMap<BookingMaterial, BookingMaterialDto>().ReverseMap();
    }
}