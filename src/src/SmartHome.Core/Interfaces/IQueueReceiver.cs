using System.Threading.Tasks;

namespace SmartHome.Core.Interfaces;

public interface IQueueReceiver
{
  Task<string> GetMessageFromQueue(string queueName);
}
