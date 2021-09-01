using System;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;
using LabCMS.FixtureDomain.Server.EventHandles;
using LabCMS.FixtureDomain.Server.Filters;
using LabCMS.FixtureDomain.Server.Models;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options=>
    options.JsonSerializerOptions.PropertyNamingPolicy=null);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "LabCMS.FixtureDomain.Server", Version = "v1" });
});
builder.Services.AddDbContext<FixtureDomainRepository>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(FixtureDomainRepository))))
    .AddSingleton<FixtureEventHandlersFactory>()
    .AddScoped<DecodeRoleFromCookieFilter>()
    .AddSingleton<EmailChannel>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LabCMS.FixtureDomain.Server v1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();
