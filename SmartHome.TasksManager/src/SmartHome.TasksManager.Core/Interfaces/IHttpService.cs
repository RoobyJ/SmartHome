using System.Threading.Tasks;

namespace SmartHome.TasksManager.Core.Interfaces;

public interface IHttpService
{
  Task<int> GetUrlResponseStatusCodeAsync(string url);
}
