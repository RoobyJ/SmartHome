using System.Threading.Tasks;

namespace SmartHome.TasksManager.Core.Interfaces;

public interface IQueueSender
{
  Task SendMessageToQueue(string message, string queueName);
}
