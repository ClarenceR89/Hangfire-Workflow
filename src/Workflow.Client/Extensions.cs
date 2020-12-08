using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using WorkflowCore.Interface;
using StackExchange.Redis;
using Workflow.Client;
using Nest;
using Elasticsearch.Net;

namespace Workflow.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflowClient(
            this IServiceCollection services,
            IConfiguration configuration,
            ConnectionMultiplexer redis,
            IHostEnvironment env)
        {
            try
            {
                services.Configure<WorkflowOptions>(configuration.GetSection(WorkflowOptions.OptionsName));
                var options = configuration.GetSection(WorkflowOptions.OptionsName).Get<WorkflowOptions>();
                var prefix = options.Prefix;
                var elasticUrl = options.ElasticUrl;
                bool disableDirectStreaming = options.DisableDirectStreaming;

                ConnectionSettings esConnection = new ConnectionSettings(new Uri(elasticUrl));

                services.AddWorkflow(cfg =>
                {
                    cfg.UseRedisPersistence(redis.Configuration, $"{prefix}");
                    cfg.UseRedisLocking(redis.Configuration);
                    cfg.UseRedisQueues(redis.Configuration, $"{prefix}-q");
                    cfg.UseRedisEventHub(redis.Configuration, $"{prefix}-events");
                    cfg.UseElasticsearch(esConnection.DisableDirectStreaming(disableDirectStreaming), $"{prefix}-workflows");
                });

                services.TryAddSingleton<IWorkflowClient, WorkflowClient>();

                return services;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
