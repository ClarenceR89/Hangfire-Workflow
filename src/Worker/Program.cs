using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Hangfire.Extensions;
using Workflow.Extensions;

namespace Worker
{
    public class Program
    {
        public static ConnectionMultiplexer Redis { get; private set; }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostingContext, services) => {
                    Redis = hostingContext.Configuration.GetRedisConnection();

                    //Add HangfireServer
                    services.AddWorkerHangfireServer(hostingContext.Configuration, Redis);
                    //Add Workflow Services
                    services.AddWorkflowServices();
                    services.AddWorkflowClient(hostingContext.Configuration, Redis, hostingContext.HostingEnvironment);

                    // Add the worker service
                    services.AddHostedService<Worker>();
                });
    }
}
