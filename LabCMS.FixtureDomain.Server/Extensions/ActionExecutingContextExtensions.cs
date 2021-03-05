using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Extensions
{
    public static class ActionExecutingContextExtensions
    {
        public static TAttribute? GetMethodAttribute<TAttribute>(this ActionExecutingContext context) where TAttribute : Attribute
           => context.ActionDescriptor.EndpointMetadata.FirstOrDefault(item => item is TAttribute) as TAttribute;

        public static IEnumerable<TAttribute> GetMethodAttributes<TAttribute>(this ActionExecutingContext context) where TAttribute : Attribute
            => context.ActionDescriptor.EndpointMetadata.Where(item => item is TAttribute).Cast<TAttribute>();
    }
}
