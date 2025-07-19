using System;
using System.ComponentModel.DataAnnotations;

namespace Store.G04.Core.Dtos.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email is Required !!")]
        [EmailAddress(ErrorMessage = "Invalid Email Format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Display Name is Required.")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Phone Number is Required.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        public string Password { get; set; }
    }
}