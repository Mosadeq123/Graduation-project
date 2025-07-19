namespace Store.G04.Core.Entities;
public class Wishlist
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; }
    public int? MaterialId { get; set; }
    public virtual RawMaterial Material { get; set; }
    public int? MachineId { get; set; }
    public virtual MachineEntity Machine { get; set; }
}

