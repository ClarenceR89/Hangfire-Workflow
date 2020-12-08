using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Hangfire.Jobs;
using Hangfire.Enqueuers;
using Hangfire.Schedulers;
using Hangfire.Heartbeat;
using Hangfire.Heartbeat.Server;
using Hangfire.Options;
using Hangfire.Redis;
using StackExchange.Redis;
using Utilities;

namespace Hangfire.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHangfireOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<HangfireOptions>(configuration.GetSection(HangfireOptions.OptionsName));
            return services;
        }

        /// <summary>
        /// AddWorkerHangfireServer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddWorkerHangfireServer(
            this IServiceCollection services,
            IConfiguration configuration,
            ConnectionMultiplexer redis)
        {
            services.AddHangfireOptions(configuration);
            HangfireOptions hfOptions = configuration.GetSection(HangfireOptions.OptionsName).Get<HangfireOptions>();

            // Add the redis connection and services
            services.AddHangfireConnection(configuration, redis);

            services.AddHangfireServer(options =>
                {
                    options.ServerName = string.Format(
                        "{0}.{1}",
                        hfOptions.JobsServer,
                        Guid.NewGuid().ToString());
                    options.Queues = new[] { "priority", "workflow", "default" };
                    options.WorkerCount = Math.Min(Environment.ProcessorCount * 5, 20);
                }
            );

            return services;
        }

        public static IServiceCollection AddHangfireConnection(
            this IServiceCollection services,
            IConfiguration configuration,
            ConnectionMultiplexer redis)
        {
            services.AddHangfireOptions(configuration);
            HangfireOptions options = configuration.GetSection(HangfireOptions.OptionsName).Get<HangfireOptions>();

            var heartBeatInterval = options.HeartBeatInterval > 0 ? options.HeartBeatInterval : 1;

            services.AddHangfire(hangfireOptions => hangfireOptions
                .UseHeartbeatPage(checkInterval: TimeSpan.FromSeconds(heartBeatInterval))
                .UseRedisStorage(redis, new RedisStorageOptions
                {
                    Prefix = options.Prefix
                })
                .UseSerializerSettings(ApiSerializerSettings.GetJsonSerializerSettings()));

            // Add Jobs, Schedulers and Enqueuers
            services.AddHangfireServices();

            return services;
        }

        public static IServiceCollection AddHangfireServices(
            this IServiceCollection services)
        {
            AddJobServices(services);
            AddEnqueuerServices(services);
            AddSchedulerServices(services);

            return services;
        }

        private static void AddSchedulerServices(this IServiceCollection services)
        {
            // Schedulers
            // TODO: Additional - Create a scheduler that works on cron
            // services.TryAddTransient<JobScheduler>();
        }

        private static IServiceCollection AddEnqueuerServices(this IServiceCollection services)
        {
            // Enqueuers
            // TODO: Implement a Enqueuer that starts a Workflow job
            // services.TryAddTransient<EnqueueWorkflowJob>();

            return services;
        }

        private static IServiceCollection AddJobServices(this IServiceCollection services)
        {
            // Jobs
            // TODO: Implement Workflow Job
            // services.TryAddTransient<IWorkflowJob, WorkflowJob>();

            return services;
        }
    }
}
