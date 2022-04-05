using Esquio.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FeatureToggler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddLogging(config=> config.AddConsole())
                .AddEsquio().AddConfigurationStore(SetUpConfiguration(), "Esquio");
            var serviceProvider = services.BuildServiceProvider();
            var featureService = serviceProvider.GetService<IFeatureService>(); 
            if (await featureService.IsEnabledAsync("Colored", "Console"))
            {
                Console.BackgroundColor = ConsoleColor.Blue;
            }
            Console.WriteLine("Hello World!");
            Console.Read();
        }
        public static IConfiguration SetUpConfiguration()
        {
            var builder= new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>();
            return builder.Build();
        }
    }
}
