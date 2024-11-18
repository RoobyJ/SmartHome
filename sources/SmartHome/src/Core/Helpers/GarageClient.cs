using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartHome.Core.Helpers;

namespace Core.Helpers;

public class GarageClient(ILogger<GarageClient> logger) : IGarageClient
{
  private const int TimeToCancel = 4000;
  private static readonly HttpClient client = new();
  private readonly ILogger<GarageClient> logger = logger;

  public async Task<TemperatureDto?> GetGarageTemperature(string ip, CancellationToken ct)
  {
    TemperatureDto? itemToReturn = null;
    var cts = new CancellationTokenSource();

    try
    {
      cts.CancelAfter(TimeToCancel);
      var response = await client.GetAsync(ClientEndpoints.Garage.Temperature(ip), cts.Token);
      var contentString = await response.Content.ReadAsStringAsync(ct);
      itemToReturn = JsonConvert.DeserializeObject<TemperatureDto>(contentString);
    }
    catch (TaskCanceledException)
    {
      return itemToReturn;
    }

    return itemToReturn;
  }

  public async Task ChangeHeaterStatus(string onOff, string ip, CancellationToken ct)
  {
    var values = new Dictionary<string, string> { { "heat", $"{onOff}" } };

    var content = new FormUrlEncodedContent(values);
    await client.PatchAsync(ClientEndpoints.Garage.Heater(ip), content, ct);
  }

  public async Task<GarageHeaterStatusDto?> GetHeaterStatus(string ip, CancellationToken ct)
  {
    GarageHeaterStatusDto? itemToReturn = null;

    try
    {
      var response = await client.GetAsync(ClientEndpoints.Garage.HeaterStatus(ip), ct);
      var contentString = await response.Content.ReadAsStringAsync(ct);
      itemToReturn = JsonConvert.DeserializeObject<GarageHeaterStatusDto>(contentString);
    }
    catch (TaskCanceledException)
    {
      return itemToReturn;
    }

    return itemToReturn;
  }
}
