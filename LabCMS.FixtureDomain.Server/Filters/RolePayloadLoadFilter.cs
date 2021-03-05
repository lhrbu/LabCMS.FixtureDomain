using LabCMS.FixtureDomain.Server.Attributes;
using LabCMS.FixtureDomain.Server.Extensions;
using LabCMS.FixtureDomain.Shared.ClientSideModels;
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

namespace LabCMS.FixtureDomain.Server.Filters
{
    public class RolePayloadLoadFilter : IAsyncActionFilter
    {
        private readonly IConfiguration _configuration;
        private readonly CookieJwtPayloadLoadService _cookieJwtPayloadLoadService;
        private readonly IMapper _mapper;
        public RolePayloadLoadFilter(
            IConfiguration configuration,
            CookieJwtPayloadLoadService cookieJwtPayloadLoadService,
            IMapper mapper)
        {
            _configuration = configuration;
            _cookieJwtPayloadLoadService = cookieJwtPayloadLoadService;
            _mapper =mapper;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            RolePayloadRequiredAttribute? attribute = context.GetMethodAttribute<RolePayloadRequiredAttribute>();
            if (attribute is not null)
            {
                try{
                    RolePayload rolePayload =_cookieJwtPayloadLoadService.Load<RolePayload>(context.HttpContext,
                        attribute.CookieName, _configuration["JwtSecret"]);
                    context.HttpContext.Items.Add(attribute.HttpContextItemName, rolePayload);
                }catch{
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            await next();
        }
    }
}
