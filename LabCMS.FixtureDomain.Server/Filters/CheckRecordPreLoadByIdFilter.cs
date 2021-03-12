using LabCMS.FixtureDomain.Server.Controllers;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.Seedwork;
using LabCMS.Seedwork.FixtureDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabCMS.FixtureDomain.Server.Models;

namespace LabCMS.FixtureDomain.Server.Filters
{
    public class CheckRecordPreLoadByIdFilter : IAsyncActionFilter
    {
        private readonly Repository _repository;
        public CheckRecordPreLoadByIdFilter(Repository repository)
        { _repository = repository; }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            (_, object? id) = context.ActionArguments.FirstOrDefault(item => item.Key == "id");
            if (id is not null)
            {
                if (context.Controller is CheckinRecordsController)
                {
                    ICheckRecord? checkRecord = await _repository.FixtureCheckinRecords.FindAsync(id);
                    if(checkRecord is null){
                        context.Result = new NotFoundObjectResult($"Check in record {id} doesn't exist.");
                        return;
                    }
                    context.HttpContext.Items.Add(nameof(CheckinRecord), checkRecord);
                }
                else if (context.Controller is CheckoutRecordsController)
                {
                    ICheckRecord? checkRecord = await _repository.FixtureCheckoutRecords.FindAsync(id);
                    if(checkRecord is null){
                        context.Result = new NotFoundObjectResult($"Check out record {id} doesn't exist.");
                        return;
                    }
                    context.HttpContext.Items.Add(nameof(CheckoutRecord), checkRecord);
                }
            }
            await next();
        }
    }
}
