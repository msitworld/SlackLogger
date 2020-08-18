using System;
using Microsoft.Extensions.DependencyInjection;
using SlackLogging.Services;

namespace SlackLogging
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddSlackLogging(
            this IServiceCollection services,
            Action<ConfigBuilder> builder)
        {
            Configs config = new Configs();
            builder(new ConfigBuilder(config));
            services.AddSingleton(config);
            services.AddHttpClient();
            services.AddTransient<ISlackClient, SlackClient>();
            services.AddTransient<ILoggerService, LoggerService>();
            return services;
        }

        public static IServiceCollection AddSlackLogging(
            this IServiceCollection services)
        {
            services.AddSingleton(new Configs());
            services.AddHttpClient();
            services.AddTransient<ISlackClient, SlackClient>();
            services.AddTransient<ILoggerService, LoggerService>();
            return services;
        }

    }

}
