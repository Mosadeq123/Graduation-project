using Microsoft.AspNetCore.Identity;

namespace Store.G04.Core.Entities.Identity;
public class AppUser : IdentityUser
{
    public string DisplayName { get; set; }
    public Address Address { get; set; }
    public int? EmailConfirmResetCode { get; set; }
    public DateTime EmailConfirmResetCodeExpiry { get; set; }
}