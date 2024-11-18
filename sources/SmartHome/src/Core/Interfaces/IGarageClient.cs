using System.Threading;
using System.Threading.Tasks;
using Core.Dtos;

namespace Core.Interfaces;

public interface IGarageClient
{
  Task<TemperatureDto?> GetGarageTemperature(string ip, CancellationToken ct);
  Task ChangeHeaterStatus(string content, string ip, CancellationToken ct);
  Task<GarageHeaterStatusDto?> GetHeaterStatus(string ip, CancellationToken ct);
}
