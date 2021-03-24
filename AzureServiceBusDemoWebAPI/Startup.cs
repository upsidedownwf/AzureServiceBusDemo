using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureServiceBusDemoWebAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureServiceBusDemoWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            string connectionstring = Configuration["AzureServiceBus:ConnectionString"];
            services.AddSingleton<IQueueClient>(serviceProvider => {
                string queueName = Configuration["AzureServiceBus:quque"];
                return new QueueClient(connectionstring, queueName); ;
            });
            services.AddSingleton<ITopicClient>(serviceProvider => {
                string topicName = Configuration["AzureServiceBus:Topic"];
                return new TopicClient(connectionstring, topicName); ;
            });
            services.AddTransient<IQueueService, QueueService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Jon-Ajumobi Olamide. (c) {DateTime.Now.Year}. Welcome to my Azure Service Bus Demo Web API.");
                });
                endpoints.MapControllers();
            });
        }
    }
}
