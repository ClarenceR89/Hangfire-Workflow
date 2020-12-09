using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Hangfire.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ConnectionMultiplexer GetRedisConnection(this IConfiguration configuration)
        {
            return ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
        }
    }
}
