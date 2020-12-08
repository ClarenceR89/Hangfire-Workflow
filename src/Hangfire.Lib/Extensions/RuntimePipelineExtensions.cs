using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Builder;
using StackExchange.Redis;
using Hangfire;
using Hangfire.Heartbeat;
using Hangfire.Heartbeat.Server;

namespace Hangfire.Extensions
{
    public static class RuntimePipelineExtensions
    {
        public static IApplicationBuilder AddHangfireDashboard(this IApplicationBuilder app)
        {
            var options = new DashboardOptions
            {
                Authorization = new[] { new DashboardNoAuthorizationFilter() },
            };
            app.UseHangfireDashboard("/hangfire", options);

            return app;
        }
    }
}
