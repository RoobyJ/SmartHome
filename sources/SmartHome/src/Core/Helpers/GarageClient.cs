using System;
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
    return await GetJson<TemperatureDto>(ClientEndpoints.Garage.Temperature(ip), ct);
  }

  /// <summary>
  ///   Sends the ON/OFF command to the garage controller.
  ///   Throws if the controller is unreachable, does not answer within the timeout,
  ///   or answers with a non-success status code, so callers never treat a rejected command as applied.
  /// </summary>
  public async Task ChangeHeaterStatus(string onOff, string ip, CancellationToken ct)
  {
    var values = new Dictionary<string, string> { { "heat", $"{onOff}" } };
    using var content = new FormUrlEncodedContent(values);
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
    cts.CancelAfter(TimeToCancel);

    using var response = await client.PatchAsync(ClientEndpoints.Garage.Heater(ip), content, cts.Token);
    response.EnsureSuccessStatusCode();
  }

  public async Task<GarageHeaterStatusDto?> GetHeaterStatus(string ip, CancellationToken ct)
  {
    return await GetJson<GarageHeaterStatusDto>(ClientEndpoints.Garage.HeaterStatus(ip), ct);
  }

  /// <summary>
  ///   GETs and deserializes a JSON body. Returns null when the controller is unreachable,
  ///   times out, answers with an error status, or returns an unparsable body.
  /// </summary>
  private async Task<T?> GetJson<T>(string url, CancellationToken ct) where T : class
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
    cts.CancelAfter(TimeToCancel);

    try
    {
      using var response = await client.GetAsync(url, cts.Token);
      if (!response.IsSuccessStatusCode)
      {
        logger.LogWarning("GET {Url} returned {StatusCode}", url, (int)response.StatusCode);
        return null;
      }

      var contentString = await response.Content.ReadAsStringAsync(cts.Token);
      return JsonConvert.DeserializeObject<T>(contentString);
    }
    catch (OperationCanceledException) when (!ct.IsCancellationRequested)
    {
      logger.LogWarning("GET {Url} timed out", url);
      return null;
    }
    catch (HttpRequestException ex)
    {
      logger.LogWarning("GET {Url} failed: {Message}", url, ex.Message);
      return null;
    }
    catch (JsonException ex)
    {
      logger.LogWarning("GET {Url} returned invalid JSON: {Message}", url, ex.Message);
      return null;
    }
  }
}
