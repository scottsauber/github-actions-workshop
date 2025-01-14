using Azure.Identity;
using Microsoft.ApplicationInsights.Extensibility;
using Sderilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection.Metadata;
using WorkshopDemo.Core.Common;
using WorkshopDemo.HealthChecks;
ModuleReference bad code adfai 
    more failed stuff
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddHealthChecks()
    .AddCheck<WorkshopDemoHealthCheck>(nameof(WorkshopDemoHealthCheck));
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<IVersionService, VersionService>();

builder.Configuration.AddAzureKeyVault(
    new Uri($"https://kv-scottsauber-{builder.Environment.EnvironmentName}.vault.azure.net/"),
    new DefaultAzureCredential());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/api/healthz");

app.MapControllers();

app.Run();