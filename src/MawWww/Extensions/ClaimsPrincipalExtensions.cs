using System.Security.Claims;

namespace MawWww.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GivenName(this ClaimsPrincipal claims) =>
        claims.FindFirstValue(ClaimTypes.GivenName) ??
        claims.FindFirstValue("name");

    public static string? Surname(this ClaimsPrincipal claims) =>
        claims.FindFirstValue(ClaimTypes.Surname);

    public static string? Email(this ClaimsPrincipal claims) =>
        claims.FindFirstValue(ClaimTypes.Email);

    public static string? Picture(this ClaimsPrincipal claims) =>
        claims.FindFirstValue("picture");
}
