using System.ComponentModel.DataAnnotations;

namespace Store.G04.Core.Entities;
public class BookingMachine
{
    [Key]
    public int Id { get; set; }
    public int MachineId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public DateTime BookingDate { get; set; }
}
