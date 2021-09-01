
using LabCMS.FixtureDomain.Server.Extensions;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LabCMS.FixtureDomain.Server.Filters;
public class DecodeRoleFromCookieFilter : IAsyncActionFilter
{
    private readonly FixtureDomainRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DecodeRoleFromCookieFilter> _logger;
    public DecodeRoleFromCookieFilter(
        FixtureDomainRepository repository,
        IConfiguration configuration,
        ILogger<DecodeRoleFromCookieFilter> logger)
    {
        _repository = repository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            Role role = await context.HttpContext.DecodeRoleJwtPayloadFromCookieAsync(
                _configuration["JwtSecret"], _repository);
            context.HttpContext.Items.Add(nameof(Role), role);
            await next();
        }
        catch(Exception exception)
        {
            _logger.LogInformation(exception,"Exception in {FilterName}:",
                nameof(DecodeRoleFromCookieFilter));
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
