#nullable enable
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartHome.Core.DTos;
using SmartHome.Core.DTOs;
using SmartHome.Core.Interfaces;

namespace SmartHome.Core.Helpers;

public class GarageClient : IGarageClient
{
  private const int TimeToCancel = 3000;
  private static readonly HttpClient client = new();
  private readonly ILogger<GarageClient> logger;

  public GarageClient(ILogger<GarageClient> logger)
  {
    this.logger = logger;
  }

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
    await client.PutAsync(ClientEndpoints.Garage.Heater(ip), content, ct);
  }

  public async Task<GarageHeaterStatusDto?> GetHeaterStatus(string ip, CancellationToken ct)
  {
    GarageHeaterStatusDto? itemToReturn = null;
    var cts = new CancellationTokenSource();

    try
    {
      cts.CancelAfter(TimeToCancel);
      var response = await client.GetAsync(ClientEndpoints.Garage.HeaterStatus(ip), cts.Token);
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
