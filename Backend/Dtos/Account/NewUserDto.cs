using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Account
{
    public class NewUserDto
    {
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
