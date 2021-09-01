using System.Security.Cryptography;
using System.Text;
using LabCMS.FixtureDomain.Server.Extensions;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace LabCMS.FixtureDomain.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly FixtureDomainRepository _repository;
    private readonly IConfiguration _configuration;
    public RolesController(FixtureDomainRepository repository,
        IConfiguration configuration)
    { 
        _repository = repository;
        _configuration = configuration;
    }

    [HttpPost]
    public async ValueTask<IActionResult> SignIn(
        [FromQuery] string userId, 
        [FromQuery] string passwordMD5)
    {
        Role? role = await _repository.Roles.FindAsync(userId);
        if(role == null) { return Unauthorized(); }
        string passwordMD5MD5 = MD5Encode(passwordMD5);
        if(role.PasswordMD5MD5==passwordMD5MD5)
        {
            HttpContext.SetRoleJwtPayloadInCookie(userId,
                _configuration["JwtSecret"]);
            return Ok();
        }
        else { return Unauthorized(); }
    }

    [HttpDelete]
    public new void SignOut()=>HttpContext.Response.Cookies.Delete(nameof(Role));

    private string MD5Encode(string input)
    {
        MD5 md5 = MD5.Create();
        byte[] passwordMD5MD5bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder stringBuilder = new();
        foreach (byte bt in passwordMD5MD5bytes)
        { stringBuilder.Append(bt.ToString("x2")); }
        return stringBuilder.ToString();
    }
}
