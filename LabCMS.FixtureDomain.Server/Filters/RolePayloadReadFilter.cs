using LabCMS.FixtureDomain.Server.Attributes;
using LabCMS.FixtureDomain.Server.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raccoon.Devkits.JwtAuthroization.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabCMS.FixtureDomain.Server.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using LabCMS.FixtureDomain.Server.Models;

namespace LabCMS.FixtureDomain.Server.Filters
{
    public class RolePayloadReadFilter : IAsyncActionFilter
    {
        private readonly IConfiguration _configuration;
        private readonly CookieJwtPayloadReadService _cookieJwtPayloadReadService;
        public RolePayloadReadFilter(
            IConfiguration configuration,
            CookieJwtPayloadReadService cookieJwtPayloadLoadService)
        {
            _configuration = configuration;
            _cookieJwtPayloadReadService = cookieJwtPayloadLoadService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            RolePayloadRequiredAttribute? attribute = context.GetMethodAttribute<RolePayloadRequiredAttribute>();
            if (attribute==null || attribute.Required)
            {
                try{
                    RolePayload rolePayload =_cookieJwtPayloadReadService.Read<RolePayload>(
                        context.HttpContext,
                        nameof(RolePayload), 
                        _configuration["JwtSecret"]);
                    context.HttpContext.Items.Add(nameof(RolePayload), rolePayload);
                }catch{
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            await next();
        }
    }
}
