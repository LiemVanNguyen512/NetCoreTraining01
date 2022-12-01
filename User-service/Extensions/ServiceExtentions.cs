using User_service.Persistence;
using User_service.Services;
using User_service.Services.Interfaces;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Configurations;
using MassTransit;
using User_service.IntergrationEvents.EventsHandler;
using Infrastructure.ScheduledJobs.Interfaces;
using Infrastructure.ScheduledJobs;

namespace User_service.Extensions
{
    public static class ServiceExtentions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfigurationSettings(configuration);
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.ConfigureMassTransit();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddConfigHangfireSerivce();
            services.ConfigureMemberDbContext(configuration);
            services.AddInfrastructureServices();
            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
            return services;
        }
        private static IServiceCollection ConfigureMemberDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnectionString");

            var builder = new MySqlConnectionStringBuilder(connectionString);
            services.AddDbContext<MemberContext>(m => m.UseMySql(builder.ConnectionString,
                ServerVersion.AutoDetect(builder.ConnectionString), e =>
                {
                    e.MigrationsAssembly("User-service");
                    e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                }));

            return services;
        }
        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            return services.AddScoped(typeof(IRepositoryBase<,,>), typeof(RepositoryBase<,,>))
                            .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                            .AddTransient<IMemberService, MemberService>()
                            .AddTransient<IScheduledJobService, HangfireService>();
        }
        private static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
                .Get<EventBusSettings>();
            services.AddSingleton(eventBusSettings);
            var hangFireSettings = configuration.GetSection(nameof(HangfireSettings))
                .Get<HangfireSettings>();
            services.AddSingleton(hangFireSettings);

            return services;
        }       
        private static void ConfigureMassTransit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>(nameof(EventBusSettings));
            if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
                throw new ArgumentNullException("EventBusSetting is not configured");

            var mqConnection = new Uri(settings.HostAddress);
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.AddConsumersFromNamespaceContaining<EnrolledEventConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection); 
                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
    }
}
