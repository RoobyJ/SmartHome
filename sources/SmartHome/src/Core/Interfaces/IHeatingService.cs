using System.Threading;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface IHeatingService
{
  Task ExecuteAsync(CancellationToken ct);
}
