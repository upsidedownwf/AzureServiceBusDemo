using AzureServiceBusModels;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureServiceBusDemoReceiver
{
    public class Main
    {
        private readonly ISubscriptionClient subscriptionClient;

        public Main(ISubscriptionClient subscriptionClient)
        {
            this.subscriptionClient = subscriptionClient;
        }
        public async Task Run()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            subscriptionClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
            Console.ReadLine();
            //await queueClient.CloseAsync();
            await subscriptionClient.CloseAsync();
        }
        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Something has occurred, {arg.Exception}");
            return Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(Message messsage, CancellationToken cancellationToken)
        {
            string body = Encoding.UTF8.GetString(messsage.Body);
            var person = JsonConvert.DeserializeObject<Person>(body);
            Console.WriteLine($"Person Received, Name: {person.Name}, Age: {person.Age}");
            await subscriptionClient.CompleteAsync(messsage.SystemProperties.LockToken);
        }
    }
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureHostConfiguration(configBuilder=> SetUpConfiguration(configBuilder)).ConfigureServices(serviceCollection => {
                ConfigureServices(serviceCollection);
            });
        public static void SetUpConfiguration(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>();
        }
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Main>();
            services.AddTransient<ISubscriptionClient>(sp =>
            {
                IConfiguration configuration = sp.GetService<IConfiguration>();
                string connectionstring = configuration["AzureServiceBus:ConnectionString"];
                string queueName = configuration["AzureServiceBus:Queue"];
                string topicName = configuration["AzureServiceBus:Topic"];
                string subscriptionName = configuration["AzureServiceBus:Subscription"];
                //queueClient = new QueueClient(connectionstring, queueName);
                return new SubscriptionClient(connectionstring, topicName, subscriptionName);
            });
        }
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, Welcome to my Azure Service Bus Receiver");
            var host= CreateHostBuilder(args: args).Build();
            var mainService=host.Services.GetService<Main>();
            await mainService.Run();
            Console.WriteLine("Thanks, Bye!");
        }
    }
}
