using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

    }
}
