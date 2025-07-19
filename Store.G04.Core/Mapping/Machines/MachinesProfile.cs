using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.G04.Core.Dtos;
using Store.G04.Core.Dtos.RawMaterials;
using Store.G04.Core.Entities;

namespace Store.G04.Core.Mapping.Machines;
public class MachinesProfile : Profile
{
    public MachinesProfile(IConfiguration configuration)
    {
        CreateMap<MachineEntity, MachineDtos>()
            .ForMember(d => d.PictureUrl, options => options.MapFrom(new MachineUrlResolver(configuration)));
        
        CreateMap<MachineEntity, MachineWithImageDto>()
           .ForMember(d => d.PictureUrl, options => options.MapFrom(new MachineUrlResolver(configuration)));

        CreateMap<MachineWithImageDto, MachineEntity>()
            .ForMember(dest => dest.RawMaterials, opt => opt.Ignore());

        CreateMap<BookingMachine, BookingMachineDto>().ReverseMap();
    }
}