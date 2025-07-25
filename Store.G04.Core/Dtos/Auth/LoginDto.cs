﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Store.G04.Core.Dtos.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        public string Password { get; set; }
    }
}