
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.Extensions;
public static class HttpContextGetRoleExtension
{
    public static Role GetRole(this HttpContext context) => (context.Items[nameof(Role)] as Role)!;
}
