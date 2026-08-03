using System.Security.Claims;
using Backend.Exceptions;

namespace Backend.Extensions
{

    public static class ClaimsPrincipalExtensions
    {
        // Note o '?' aqui no ClaimsPrincipal? user
        public static int ObterId(this ClaimsPrincipal? user)
        {
            var claimValue = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimValue) || !int.TryParse(claimValue, out int id))
            {
                throw new UnauthorizedException("Usuário não identificado.");
            }

            return id;
        }
    }
}


