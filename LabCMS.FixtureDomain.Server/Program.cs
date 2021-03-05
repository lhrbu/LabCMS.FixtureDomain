using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LabCMS.FixtureDomain.Server.Services;
using LabCMS.FixtureDomain.Shared;
using LabCMS.Seedwork.FixtureDomain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace LabCMS.FixtureDomain.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration(builder=>builder.AddJsonFile("yearusedindices.json",false,true))
                .UseSerilog((context, config) =>
                    config.MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft",LogEventLevel.Information)
                        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.Logger(lc => lc
                            .Filter.ByIncludingOnly(Matching.WithProperty<string>(
                                "CheckRecordType", p=>p==nameof(CheckoutRecord))
                            ).WriteTo.File("logs/checkoutrecord.log"))
                        .WriteTo.Logger(lc => lc
                            .Filter.ByIncludingOnly(Matching.WithProperty<string>(
                                "CheckRecordType", p => p == nameof(CheckinRecord))
                            ).WriteTo.File("logs/checkinrecord.log"))
                        .WriteTo.Console(LogEventLevel.Warning)
                        .WriteTo.File("logs/rollinglog.log", LogEventLevel.Information,rollingInterval:RollingInterval.Month)
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
