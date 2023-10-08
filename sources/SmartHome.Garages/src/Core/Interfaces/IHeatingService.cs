using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.Core.Interfaces;

public interface IHeatingService
{
  Task ExecuteAsync(CancellationToken ct);
}
