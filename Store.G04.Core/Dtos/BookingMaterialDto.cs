namespace Store.G04.Core.Dtos;
public class BookingMaterialDto
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    public int Qantity { get; set; }
    public string StitchLength { get; set; }
    public int? MachineId { get; set; }
}
