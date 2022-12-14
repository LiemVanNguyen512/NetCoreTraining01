using Course_service.Persistence;
using Course_service.Services;
using Course_service.Services.Interfaces;
using Infrastructure.ApiClients;
using Infrastructure.ApiClients.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Policies;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Shared.Constants;
using Common.Logging;
using Infrastructure.Extensions;
using Shared.Configurations;
using MassTransit.Caching;
using System.Runtime;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using EvenBus.Message.IntegrationEvents.Interfaces;

namespace Course_service.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfigurationSettings(configuration);
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            // configure Mass Transit
            services.ConfigureMassTransit();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.ConfigureCourseDbContext(configuration);
            services.AddInfrastructureServices();
            services.ConfigureHttpClient(configuration);
            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
            return services;
        }        
        private static IServiceCollection ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(SystemConstants.UserService, client =>
            {
                client.BaseAddress = new Uri(configuration[SystemConstants.AppSettings.UserServiceAddress]);
            }).AddHttpMessageHandler<LoggingDelegatingHandler>()
                .UseLinearHttpRetryPolicy(int.Parse(configuration[SystemConstants.AppSettings.RetryCount]),
                                        int.Parse(configuration[SystemConstants.AppSettings.RetryAttemptSeconds]));
            return services;
        }
        private static IServiceCollection ConfigureCourseDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnectionString");

            var builder = new MySqlConnectionStringBuilder(connectionString);
            services.AddDbContext<CourseContext>(m => m.UseMySql(builder.ConnectionString,
                ServerVersion.AutoDetect(builder.ConnectionString), e =>
                {
                    e.MigrationsAssembly("Course-service");
                    e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                }));

            return services;
        }
        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            return services.AddScoped(typeof(IRepositoryBase<,,>), typeof(RepositoryBase<,,>))
                            .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))                          
                            .AddTransient(typeof(ICourseService), typeof(CourseService))
                            .AddTransient(typeof(IEnrollmentService), typeof(EnrollmentService))
                            .AddTransient(typeof(IUserApiClient), typeof(UserApiClient))
                            .AddTransient<LoggingDelegatingHandler>();       
        }
        private static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
                .Get<EventBusSettings>();
            services.AddSingleton(eventBusSettings);          

            return services;
        }
        private static void ConfigureMassTransit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>(nameof(EventBusSettings));
            if (settings == null || string.IsNullOrEmpty(settings.HostAddress)) 
                throw new ArgumentNullException("EventBusSettings is not configured!");
            //Host to connect RabbitMQ
            var mqConnection = new Uri(settings.HostAddress);

            //KebabCase: UpdateMemberBalance ==> update-member-balance
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                });
                // Publish submit message, instead of sending it to a specific queue directly.
                config.AddRequestClient<IEnrolledEvent>();
            });
        }
    }
}
