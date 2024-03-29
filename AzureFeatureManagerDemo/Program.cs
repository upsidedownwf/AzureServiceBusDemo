using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureFeatureManagerDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var settings = config.Build();
                        config.AddAzureAppConfiguration(options =>
                        {
                            options.Connect(settings.GetConnectionString("AppConfig"))
                            .UseFeatureFlags(refresh => {
                                refresh.CacheExpirationInterval = new TimeSpan(0, 0, 10);
                                refresh.Label = "SubDemo";
                                });
                            //.ConfigureRefresh(refresh =>
                            //{
                            //    //refresh.Register("", true)
                            //    //.SetCacheExpiration(new TimeSpan(0, 5, 0));
                            //});
                        });
                    })
                    .UseStartup<Startup>();
                });
    }
}
