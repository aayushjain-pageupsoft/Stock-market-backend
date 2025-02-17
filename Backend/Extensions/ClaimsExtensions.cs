using System.Security.Claims;

namespace Backend.Extensions
{
    public static class ClaimsExtensions
    {
        /// <summary>
        /// Retrieves the username (given name) from the claims of the specified user.
        /// </summary>
        /// <param name="user">The ClaimsPrincipal representing the user.</param>
        /// <returns>The username (given name) if found; otherwise, an empty string.</returns>
        public static string GetUsername(this ClaimsPrincipal user)
        {
            // Retrieve the username (given name) from the claims of the specified user.
            var claim = user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"));
            return claim?.Value ?? string.Empty;
        }
    }
}