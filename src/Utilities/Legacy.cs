// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.HttpsPolicy;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using StackExchange.Redis;
// using Hangfire.Lib.Extensions;
// using Workflow.Lib.Extensions;
// using Swashbuckle.AspNetCore.Swagger;

// namespace Dashboard
// {
//     public class Startup
//     {
//         public static ConnectionMultiplexer Redis;
//         public Startup(IConfiguration configuration)
//         {
//             Configuration = configuration;
//             Redis = configuration.GetRedisConnection();
//         }

//         public IConfiguration Configuration { get; }

//         // This method gets called by the runtime. Use this method to add services to the container.
//         public void ConfigureServices(IServiceCollection services)
//         {
//             services.AddOptions();
//             services.AddHangfireConnection(Redis);
//             services.AddWorkflowServices();
//             services.AddSwaggerGen(config => {
//                 config.SwaggerDoc("v1", new Info { Title = "Workflow API", Version = "v1" });
//             });
//             services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
//         }

//         // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//         public void Configure(IApplicationBuilder app, IHostingEnvironment env)
//         {
//             if (env.IsDevelopment())
//             {
//                 app.UseDeveloperExceptionPage();
//             }
//             else
//             {
//                 // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                 app.UseHsts();
//             }

//             app.AddHangfireDashboard();
//             app.UseSwagger();
//             app.UseSwaggerUI(config => {
//                 config.SwaggerEndpoint("/swagger/v1/swagger.json", "Workflow Service");
//             });
//             app.UseHttpsRedirection();
//             app.UseMvc();
//         }
//     }
// }
