using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        /// <summary>
        /// Initializes configuration settings and the symmetric security key used for signing tokens.
        /// </summary>
        /// <param name="config">IConfiguration instance containing app settings, including JWT keys.</param>
        public TokenService(IConfiguration config)
        {
            _config = config;
            // Generate a symmetric security key using the secret key from the app configuration.
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            
        }

        /// <summary>
        /// Creates a JWT token for the given user.
        /// </summary>
        /// <param name="user">The AppUser object containing user details.</param>
        /// <returns>A JWT token as a string.</returns>
        public string CreateToken(AppUser user)
        {
            // Create a list of claims (user identity information to be stored in the token)
            var claims = new List<Claim>
           {
                // Store the user's email in the token
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
               // Store the user's username in the token
               new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
           };
            // Generate signing credentials using the security key and HMAC SHA512 algorithm
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            // Define token properties (payload details)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // Attach claims (user information)
                Expires = DateTime.Now.AddDays(1), // Set expiration time (valid for 1 day)
                SigningCredentials = creds, // Attach signing credentials (to verify token authenticity)
                Issuer = _config["JWT:Issuer"], // Token issuer (typically the backend server URL)
                Audience = _config["JWT:Audience"] // Intended audience (who should accept this token)
            };
            // Create a JWT token handler instance
            var tokenHandler = new JwtSecurityTokenHandler();
            // Generate the security token using the token descriptor
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // Convert the token into a readable string format and return it
            return tokenHandler.WriteToken(token);
        }
    }
}
