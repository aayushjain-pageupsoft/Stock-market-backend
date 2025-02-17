using Backend.Models;

namespace Backend.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Create a token for a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string CreateToken(AppUser user);
    }
}
