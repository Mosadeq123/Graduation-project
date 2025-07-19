using System.ComponentModel.DataAnnotations;

namespace Store.G04.Core.Dtos.Auth
{
    public class ResetCodeConfiramtionByEmailDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required int ResetCode { get; set; }
    }
}
