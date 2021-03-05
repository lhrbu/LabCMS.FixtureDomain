using LabCMS.FixtureDomain.Server.Attributes;
using LabCMS.FixtureDomain.Server.Extensions;
using LabCMS.FixtureDomain.Server.Policies;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared;
using LabCMS.FixtureDomain.Shared.ClientSideModels;
using LabCMS.Seedwork;
using LabCMS.Seedwork.FixtureDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Filters
{
    public class PermissionPolicyValidateFilter : IAsyncActionFilter
    {
        private readonly Repository _repository;
        private readonly IServiceProvider _serviceProvider;
        public PermissionPolicyValidateFilter(
            Repository repository,
            IServiceProvider serviceProvider)
        { 
            _repository = repository;
            _serviceProvider = serviceProvider;
        }

        private async ValueTask<bool> ValidateTestFieldResponse(RolePayload rolePayload,CheckoutRecord checkoutRecord)
        {
            await _repository.Entry(checkoutRecord).Reference(item=>item.Fixture).LoadAsync();
            return rolePayload.ResponseTestFieldName == checkoutRecord.Fixture!.TestFieldName;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            PermissionPolicyAttribute? attribute = context.GetMethodAttribute<PermissionPolicyAttribute>();
            if (attribute is not null)
            {

                RolePayload rolePayload = (context.HttpContext.Items[nameof(RolePayload)] as RolePayload)!;
                int id = (int)context.HttpContext.Items["id"]!;
                ICheckRecord checkRecord = (context.HttpContext.Items[id] as ICheckRecord)!;

                IEnumerable<IPermissionPolicy> permissionPolicies = attribute.PolicyTypes
                    .Select(type=>(_serviceProvider.GetRequiredService(type) as IPermissionPolicy)!);

                foreach(IPermissionPolicy permissionPolicy in permissionPolicies)
                {
                    if (!await permissionPolicy.ValidateAsync(rolePayload, checkRecord))
                    {
                        context.Result = new UnauthorizedResult(); return;
                    }
                }
            }
            await next();
        }
    }
}
