using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.G04.Core.Dtos.RawMaterials;

namespace Store.G04.Core.Mapping
{
    public class PictureUrlResolver : IValueResolver<RawMaterial, RawMaterialDtos, string>
    {
        private readonly IConfiguration _configuration;

        public PictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(RawMaterial source, RawMaterialDtos destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
            {
                return string.Empty; // Return an empty string if PictureUrl is null or empty
            }

            return $"{_configuration["BaseURL"]}{source.PictureUrl}";
        }
    }
}