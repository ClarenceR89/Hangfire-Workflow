using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;
using WorkflowCore;

namespace Workflow.Lib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflowWorker(this IServiceCollection services, string redisConnection)
        {
            services.AddWorkflow(cfg =>
            {
                cfg.UseRedisPersistence(redisConnection, "app-name");
                cfg.UseRedisLocking(redisConnection);
                cfg.UseRedisQueues(redisConnection, "app-name");
                cfg.UseRedisEventHub(redisConnection, "channel-name");
            });

            return services;
        }

        public static IServiceCollection AddWorkflowServices(this IServiceCollection services, string redisConnection)
        {
            services.AddWorkflow(cfg =>
            {
                cfg.UseRedisPersistence(redisConnection, "app-name");
            });

            services.AddSingleton<IWorkflowService, WorkflowService>();

            return services;
        }
    }
}
