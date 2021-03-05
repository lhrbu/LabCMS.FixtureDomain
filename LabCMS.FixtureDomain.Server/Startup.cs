using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LabCMS.FixtureDomain.Server.Controllers;
using LabCMS.FixtureDomain.Server.Filters;
using LabCMS.FixtureDomain.Server.Models;
using LabCMS.FixtureDomain.Server.Policies;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Server.Services;
using LabCMS.FixtureDomain.Shared;
using LabCMS.Seedwork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Npgsql;
using Raccoon.Devkits.EmailToolkit;
using Raccoon.Devkits.JwtAuthorization;
using Raccoon.Devkits.JwtAuthroization.Services;
using Serilog;

namespace LabCMS.FixtureDomain.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureFilters(IServiceCollection services)
        {
            services.AddTransient<CheckRecordLogFilter>();
            services.AddTransient<RolePayloadLoadFilter>();

            services.AddScoped<CheckRecordFindByIdFilter>();
            services.AddScoped<PermissionPolicyValidateFilter>();
        }

        public void ConfigurePolicies(IServiceCollection services)
        {
            services.AddTransient<ApplicantOnlyPolicy>();
            services.AddTransient<TestFieldResponseAuthorizePolicy>();
            services.AddTransient<FixtureRoomAuthorizePolicy>();
            services.AddTransient<ScannerOnlyPolicy>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddJsonOptions(options=>options.JsonSerializerOptions.PropertyNamingPolicy=null);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LabCMS.FixtureDomain.Server", Version = "v1" });
            });
            services.AddDbContextPool<Repository>(options => {
                options.UseNpgsql(Configuration.GetConnectionString(nameof(Repository)));
                options.LogTo(log => Log.Information(log), LogLevel.Warning);
            },64);
            services.AddDbContext<FixtureYearUsedIndicesRepository>(options =>
                options.UseNpgsql(Configuration.GetConnectionString(nameof(FixtureYearUsedIndicesRepository))));
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddTransient<JwtEncodeService>();
            services.AddTransient<CookieJwtPayloadLoadService>();
            services.AddTransient<IFixtureIndexGenerator, DatabaseFixtureIndexGenerator>();
            services.AddTransient<IFixtureNoGenerator, FixtureNoGenerator>();
            services.AddTransient<RandomPasswordGenerator>();

            services.AddEmailSendService("****", "****", "*****");

            ConfigureFilters(services);
            ConfigurePolicies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LabCMS.FixtureDomain.Server v1"));

            }
            
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            Log.Warning("ASP NET is listenning on {@IPAddresses}",
                    app.ServerFeatures.Get<IServerAddressesFeature>().Addresses);
            
        }
    }
}
