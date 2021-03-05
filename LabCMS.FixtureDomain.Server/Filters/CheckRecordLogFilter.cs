using LabCMS.FixtureDomain.Shared;
using LabCMS.Seedwork.FixtureDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Filters
{
    public class CheckRecordLogFilter : IAsyncActionFilter
    {
        private readonly ILogger<CheckRecordLogFilter> _logger;
        public CheckRecordLogFilter(ILogger<CheckRecordLogFilter> logger) => _logger = logger;

        private static Dictionary<string, object> _checkOutRecordTypeToken = new() { ["CheckRecordType"] = nameof(CheckoutRecord) };
        private static Dictionary<string, object> _checkInRecordTypeToken = new() { ["CheckRecordType"] = nameof(CheckinRecord) };
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            object? result = (await next()).Result;
            if(result is ObjectResult objectResult && objectResult.Value is ICheckRecord checkRecord)
            {
                Dictionary<string, object> checkRecordTypeToken = checkRecord switch
                {
                   CheckoutRecord => _checkOutRecordTypeToken,
                   CheckinRecord => _checkInRecordTypeToken,
                    _ => throw new InvalidCastException()
                };
                using var scope = _logger.BeginScope(checkRecordTypeToken);
                _logger.LogInformation("Check Record {Id} {Status}:{@CheckRecord}",
                    checkRecord.Id,checkRecord.Status,checkRecord);
            }
        }
    }
}
