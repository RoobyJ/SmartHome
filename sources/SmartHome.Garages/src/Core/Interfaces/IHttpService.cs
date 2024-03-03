using System.Threading.Tasks;

namespace Core.Interfaces;

public interface IHttpService
{
  Task<int> GetUrlResponseStatusCodeAsync(string url);
}
