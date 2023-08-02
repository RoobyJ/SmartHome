using System.Threading.Tasks;

namespace SmartHome.TasksManager.Core.Interfaces;

public interface IQueueReceiver
{
  Task<string> GetMessageFromQueue(string queueName);
}
