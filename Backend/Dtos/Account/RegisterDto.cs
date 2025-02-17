using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Dtos.Account
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username cannot be empty.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email cannot be empty.")]
        [EmailAddress]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email already in use")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty.")]
        public string? Password { get; set; }

    }
}
