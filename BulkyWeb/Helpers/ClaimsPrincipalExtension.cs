using System.Security.Claims;

namespace BulkyWeb.Helpers
{
    internal static class ClaimsPrincipalExtension
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            ClaimsIdentity? claims = (ClaimsIdentity)user.Identity;
            string? UserId = claims?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (UserId == null) { return ""; }
            return UserId;
        }
    }
}
