using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
