using System.Threading;
using System.Threading.Tasks;
using SmartHome.Core.DTos;
using SmartHome.Core.DTOs;

namespace Core.Interfaces;

public interface IGarageClient
{
  Task<TemperatureDto> GetGarageTemperature(string ip, CancellationToken ct);
  Task ChangeHeaterStatus(string content, string ip, CancellationToken ct);
  Task<GarageHeaterStatusDto> GetHeaterStatus(string ip, CancellationToken ct);
}
