#nullable enable
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartHome.Core.DTos;
using SmartHome.Core.DTOs;

namespace SmartHome.Core.Helpers;

public abstract class GarageClient
{
  private static readonly HttpClient client = new();
  
  public static async Task<TemperatureDto?> GetGarageTemperature(string ip, CancellationToken ct)
  {
    TemperatureDto? itemToReturn = null;
    try
    {
      var response = await client.GetAsync(ClientEndpoints.Garage.Temperature(ip), ct);
      var contentString = await response.Content.ReadAsStringAsync(ct);
      itemToReturn = JsonConvert.DeserializeObject<TemperatureDto>(contentString);
    }
    catch (Exception ex)
    {
      Console.Write(ex);
    }

    return itemToReturn;
  }

  public static async Task ChangeHeaterStatus(string onOff, string ip, CancellationToken ct)
  {
    var values = new Dictionary<string, string> { { "heat", $"{onOff}" } };

    var content = new FormUrlEncodedContent(values);
    await client.PutAsync(ClientEndpoints.Garage.Heater(ip), content, cancellationToken: ct);
  }

  public static async Task<GarageHeaterStatusDto?> GetHeaterStatus(string ip, CancellationToken ct)
  {
    var response = await client.GetAsync(ClientEndpoints.Garage.HeaterStatus(ip), ct);
    var contentString = await response.Content.ReadAsStringAsync(ct);
    return JsonConvert.DeserializeObject<GarageHeaterStatusDto>(contentString);
  }
}
