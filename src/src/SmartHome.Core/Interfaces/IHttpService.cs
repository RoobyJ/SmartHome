using System.Threading.Tasks;

namespace SmartHome.Core.Interfaces;

public interface IHttpService
{
  Task<int> GetUrlResponseStatusCodeAsync(string url);
}
