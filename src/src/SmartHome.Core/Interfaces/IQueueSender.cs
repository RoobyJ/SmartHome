using System.Threading.Tasks;

namespace SmartHome.Core.Interfaces;

public interface IQueueSender
{
  Task SendMessageToQueue(string message, string queueName);
}
