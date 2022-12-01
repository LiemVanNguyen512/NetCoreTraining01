﻿using Common.Logging;
using Hangfire;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Configurations;

namespace User_service.Extensions
{
    public static class HostExtensions
    {
        public static void AddAppConfigurations(this ConfigureHostBuilder host)
        {
            host.ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            }).UseSerilog(Serilogger.Configure);
        }
        internal static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
        {
            var configDashboard = configuration.GetSection("HangFireSettings:Dashboard").Get<DashboardOptions>();
            var hangfireSettings = configuration.GetSection("HangFireSettings").Get<HangfireSettings>();
            var hangfireRoute = hangfireSettings.Route;

            app.UseHangfireDashboard(hangfireRoute, new DashboardOptions
            {
                //Authorization = new[] { new AuthorizationFilter() },
                DashboardTitle = configDashboard.DashboardTitle,
                StatsPollingInterval = configDashboard.StatsPollingInterval,
                AppPath = configDashboard.AppPath,
                IgnoreAntiforgeryToken = true
            });

            return app;
        }
    }
}
