using System.ComponentModel.DataAnnotations;

namespace Store.G04.Core.Dtos.Auth
{
    public class ForgetPasswordRequestByEmailDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
