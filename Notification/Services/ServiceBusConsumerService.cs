
using Azure.Messaging.ServiceBus;
using Core.Dtos;

namespace Notification.Services;

public class ServiceBusConsumerService {

    private readonly ServiceBusClient _client;
    private readonly SmsService _smsService;
    private const string QueueName = "user-created";

    public ServiceBusConsumerService(string connectionString, SmsService smsService)
    {
        _client = new ServiceBusClient(connectionString);
        _smsService = smsService;
    }

    public void StartProcessingMessages()
    {
        // Get a processor for the queue
        ServiceBusProcessor processor = _client.CreateProcessor(QueueName, new ServiceBusProcessorOptions());

        // Add handlers for processing messages and handling errors
        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;

        // Start the processor
        processor.StartProcessingAsync();
    }

    private Task MessageHandler(ProcessMessageEventArgs args)
    {
        string message = args.Message.Body.ToString();
        Console.WriteLine($"Received message: {message}");
        var user = UserDto.FromJson(message);
        Console.WriteLine($"User: {user.PhoneNumber} - {user.VerificationCode}");

        if (!string.IsNullOrEmpty(user.PhoneNumber)) {
            try {
                _smsService.SendSms(user.PhoneNumber, $"Your verification code is {user.VerificationCode}");
            } catch (Exception ex) {
                Console.WriteLine($"Error sending SMS: {ex.Message}");
            }
        }

        return Task.CompletedTask;
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Message handler encountered an exception: {args.Exception}.");
        return Task.CompletedTask;
    }
}