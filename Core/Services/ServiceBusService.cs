using Azure.Messaging.ServiceBus;
using Core.Services.Interfaces;

namespace Core.Services;

public class ServiceBusService : IQueueService
{
    private readonly ServiceBusClient _serviceBusClient;

    public ServiceBusService(string connectionString)
    {
        _serviceBusClient = new ServiceBusClient(connectionString);        
    }

    public async Task SendMessage(string queue, string message)
    {
        var sender = _serviceBusClient.CreateSender(queue);
        await sender.SendMessageAsync(new ServiceBusMessage(message));
    }
}