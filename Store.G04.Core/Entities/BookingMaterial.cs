namespace Store.G04.Core.Entities;
public class BookingMaterial: BaseEntity<int>
{
    public int MaterialId { get; set; }
    public virtual RawMaterial Material { get; set; }

    public int Qantity { get; set; }
    public string StitchLength { get; set; }
    public int? MachineId { get; set; }
    public virtual MachineEntity Machine { get; set; }
}
