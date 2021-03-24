using AzureServiceBusModels;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureServiceBusDemoReceiver
{
    class Program
    {
        private static IQueueClient queueClient;
        private static ISubscriptionClient subscriptionClient;
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, Welcome to my Azure Service Bus Receiver");
            var configuration = GetConfiguration();
            string connectionstring = configuration["AzureServiceBus:ConnectionString"];
            string queueName = configuration["AzureServiceBus:Queue"];
            string topicName = configuration["AzureServiceBus:Topic"];
            string subscriptionName = configuration["AzureServiceBus:Subscription"];
            //queueClient = new QueueClient(connectionstring, queueName);
            subscriptionClient = new SubscriptionClient(connectionstring, topicName, subscriptionName);
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            subscriptionClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
            Console.ReadLine();
            await queueClient.CloseAsync();
        }

        private static async Task ProcessMessageAsync(Message messsage, CancellationToken cancellationToken)
        {
            string body = Encoding.UTF8.GetString(messsage.Body);
            var person = JsonConvert.DeserializeObject<Person>(body);
            Console.WriteLine($"Person Received, Name: {person.Name}, Age: {person.Age}");
            await subscriptionClient.CompleteAsync(messsage.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Something has occurred, {arg.Exception}");
            return Task.CompletedTask;
        }

        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>();
            return builder.Build();
        }
    }
}
