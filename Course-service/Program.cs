using Common.Logging;
using Course_service.Extensions;
using Course_service.Persistence;
using Infrastructure.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {builder.Environment.ApplicationName} up");

// Add services to the container.
builder.Host.ConfigureAppConfiguration((context, config) =>
{
    var env = context.HostingEnvironment;
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();
}).UseSerilog(Serilogger.Configure);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
//app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandler>();
//app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.MigrateDatabase<CourseContext>((context, _) =>
{
    CourseContextSeed.SeedCouresAsync(context, Log.Logger).Wait();
}).Run();
