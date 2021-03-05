using LabCMS.FixtureDomain.Server.Controllers;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared;
using LabCMS.Seedwork;
using LabCMS.Seedwork.FixtureDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Filters
{
    public class CheckRecordFindByIdFilter : IAsyncActionFilter
    {
        private readonly Repository _repository;
        public CheckRecordFindByIdFilter(Repository repository)
        { _repository = repository; }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            (_, object? id) = context.ActionArguments.FirstOrDefault(item => item.Key == "id");
            if (id is not null)
            {
                context.HttpContext.Items.Add("id", id);
                if (context.Controller is CheckinRecordsController)
                {
                    ICheckRecord? checkRecord = await _repository.FixtureCheckinRecords.FindAsync(id);
                    if(checkRecord is null){
                        context.Result = new NotFoundObjectResult($"Check in record {id} doesn't exist.");
                        return;
                    }
                    context.HttpContext.Items.Add(id, checkRecord);
                }
                else if (context.Controller is CheckoutRecordsController)
                {
                    ICheckRecord? checkRecord = await _repository.FixtureCheckoutRecords.FindAsync(id);
                    if(checkRecord is null){
                        context.Result = new NotFoundObjectResult($"Check out record {id} doesn't exist.");
                        return;
                    }
                    context.HttpContext.Items.Add(id, checkRecord);
                }
            }
            await next();
        }
    }
}
