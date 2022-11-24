using Common.Logging;
using User_service.Extensions;
using User_service.Persistence;
using Infrastructure.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {builder.Environment.ApplicationName} up");
// Add services to the container.
builder.Host.AddAppConfigurations();
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

app.MigrateDatabase<MemberContext>((context, _) =>
{
    MemberContextSeed.SeedMemberAsync(context, Log.Logger).Wait();
}).Run();
