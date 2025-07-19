namespace Store.G04.Core.Entities;
public class WishlistItemEnitiey
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
