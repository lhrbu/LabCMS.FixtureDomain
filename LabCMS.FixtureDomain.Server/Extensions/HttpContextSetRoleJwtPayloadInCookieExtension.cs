
using Jose;
using LabCMS.FixtureDomain.Shared.Models;
using Microsoft.AspNetCore.DataProtection;

namespace LabCMS.FixtureDomain.Server.Extensions;
public static class HttpContextSetRoleJwtPayloadInCookieExtension
{
    public static void SetRoleJwtPayloadInCookie(this HttpContext context, string userId, string jwtSecret)
{
        string token = Jose.JWT.Encode(new { UserId = userId }, 
                    jwtSecret,
                    JweAlgorithm.PBES2_HS256_A128KW,
                    JweEncryption.A256CBC_HS512);
        context.Response.Cookies.Append(nameof(Role), token);
    }
}
