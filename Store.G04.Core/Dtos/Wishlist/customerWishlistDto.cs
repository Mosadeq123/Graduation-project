using System.Collections.Generic;

namespace Store.G04.Core.Dtos.Wishlist
{
    public class customerWishlistDto
    {
        public string Id { get; set; }
        public List<WishlistItemDto> Items { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }

    public class WishlistItemDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? MaterialId { get; set; }
        public int? MachineId { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}