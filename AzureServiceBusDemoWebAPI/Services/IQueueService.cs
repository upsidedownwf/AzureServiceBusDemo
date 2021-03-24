using AzureServiceBusModels;
using System.Threading.Tasks;

namespace AzureServiceBusDemoWebAPI.Services
{
    public interface IQueueService
    {
        Task SendMessage<T>(T message) where T : Person;
    }
}