using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartHome.Core.DTos;
using SmartHome.Core.DTOs;

namespace SmartHome.Core.Helpers;

public class GarageClient
{
  private static readonly HttpClient Client = new();

  public static async Task<TemperatureDto> GetGarageTemperature(string ip)
  {
    var response = await Client.GetAsync(ClientEndpoints.Garage.Temperature(ip));
    var contentString = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<TemperatureDto>(contentString);
  }

  public static async Task ChangeHeaterStatus(string onOff, string ip)
  {
    var values = new Dictionary<string, string> { { "heat", $"{onOff}" } };

    var content = new FormUrlEncodedContent(values);
    await Client.PutAsync(ClientEndpoints.Garage.Heater(ip), content);
  }

  public static async Task<GarageHeaterStatusDto> GetHeaterStatus(string ip)
  {
    var response = await Client.GetAsync(ClientEndpoints.Garage.HeaterStatus(ip));
    Console.Write(response);
    var contentString = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<GarageHeaterStatusDto>(contentString);
  }
}
