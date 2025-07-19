using AutoMapper;
using Store.G04.Core.Dtos;
using Store.G04.Core.Dtos.Wishlist;
using Store.G04.Core.Entities;

namespace Store.G04.Core.Mapping.Wishlist;
public class WishlistProfile : Profile
{
    public WishlistProfile()
    {
        CreateMap<CustomerWishlist, customerWishlistDto>().ReverseMap();
        CreateMap<WishlistItemEnitiey, WishlistItemDto>().ReverseMap();
        CreateMap<Entities.Wishlist, WishlistDto>().ReverseMap();
    }
}
