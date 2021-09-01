
using Jose;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.Extensions;
public static class HttpContextDecodeRoleJwtPayloadFromCookieExtension
{
    private record RolePayload(string UserId);
    public static async ValueTask<Role> DecodeRoleJwtPayloadFromCookieAsync(
        this HttpContext context, string jwtSecret,
        FixtureDomainRepository repository)
    {
        string? token = context.Request.Cookies[nameof(Role)];
        if (token == null)
        { throw new InvalidOperationException("HttpContext Request doesn't contain Role cookie item.");}
        RolePayload payload = Jose.JWT.Decode<RolePayload>(token,
                        jwtSecret,
                        JweAlgorithm.PBES2_HS256_A128KW,
                        JweEncryption.A256CBC_HS512);
        return await repository.Roles.FindAsync(payload.UserId) ??
            throw new InvalidOperationException($"UserId: {payload.UserId} doesn't exists ");

    }
}
