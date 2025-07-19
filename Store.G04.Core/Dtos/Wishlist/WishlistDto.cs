namespace Store.G04.Core.Dtos.Wishlist;
public class WishlistDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public int? MaterialId { get; set; }
    public int? MachineId { get; set; }
}
