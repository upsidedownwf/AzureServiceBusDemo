using AzureServiceBusModels;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusDemoWebAPI.Services
{
    public class QueueService : IQueueService
    {
        private readonly ITopicClient topicClient;

        public QueueService(ITopicClient topicClient)
        {
            this.topicClient = topicClient;
        }
        public Task SendMessage<T>(T message) where T : Person
        {
            string messageBody = JsonConvert.SerializeObject(message);
            var servicebusmessage = new Message(Encoding.UTF8.GetBytes(messageBody)) { 
                ContentType= ""
            };
            //A topic would send a message to all its subscribers unless a filter is applied to send to some specific suscriber(s)
            //when a topic has more than one subscriber you can set a KVP filter on each subscription on azure portal
            //The commented below for subscriptions with a filter set on azure as Key: messgeType, Value: Person
            //But it is not required here because the Topic has only one susbcriber ast at the time of writing.
            //servicebusmessage.UserProperties["messageType"] = typeof(T).Name;
            return topicClient.SendAsync(servicebusmessage);
        }
    }
}
