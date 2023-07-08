
namespace Core.Services.Interfaces;

public interface IQueueService
{
    Task SendMessage(string queue, string message);
}