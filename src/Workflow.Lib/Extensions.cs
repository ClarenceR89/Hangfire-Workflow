using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using WorkflowCore.Interface;
using Workflow.Client;
using Workflow.Steps;
using Workflow.Workflows;
using Nest;
using Elasticsearch.Net;

namespace Workflow.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflowServices(this IServiceCollection services)
        {
            // Add StepBody and StepBodyAsync that require IoC to the service collection here
            services.TryAddTransient<ConsoleLogStep>();
            services.TryAddTransient<EndWorkflowStep>();
            services.TryAddTransient<StartWorkflowStep>();

            return services;
        }
    }
    public static class RuntimePipelineExtensions
    {
        public static IApplicationBuilder AddWorkflowHost(this IApplicationBuilder app)
        {
            var host = app.ApplicationServices.GetService<IWorkflowHost>();

            host.AddWorkflows();

            return app;
        }

        public static IWorkflowHost AddWorkflows(this IWorkflowHost host)
        {
            host.RegisterWorkflow<HelloWorld>();

            return host;
        }
    }
}
