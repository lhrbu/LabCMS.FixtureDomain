using System;
using LabCMS.FixtureDomain.Server;
using LabCMS.FixtureDomain.Shared.ClientSideModels;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LabCMS.FixtureDomain.UnitTest
{
    public static class TestEnviroment
    {
        public static int RandomIndex {get;} = new Random().Next();
        public static TestServer Instance { get; } = new(WebHost.CreateDefaultBuilder()
            .UseStartup<Startup>().UseEnvironment("Development"));
        public static IServiceProvider CreateScopedServiceProvider()=>Instance.Services.CreateScope().ServiceProvider;
        
        public static CheckoutRecordPayload SeedCheckoutRecordInClient {get;} = new()
        {
            FixtureNo = 10000,
            ReceiverCompany = $"REC_{RandomIndex}",
            Receiver = $"RE_{RandomIndex}",
            PlanndReturnDate = DateTimeOffset.Now
        };
    }
}