using Microsoft.AspNetCore.Mvc;
using SmartHome.Core.Entities;
using System.Text.Json;
using SmartHome.Core.DTos;
using SmartHome.Core.DTOs;
using SmartHome.Core.Helpers;
using SmartHome.Core.Interfaces;
using SmartHome.Core.Models;

namespace SmartHome.webapi.Controllers;

[Route("garage/{id:int}")]
public class GarageController : ApiControllerBase
{
  private readonly ILogger<GarageController> _logger;
  private readonly IGarageService _garageService;

  public GarageController(ILogger<GarageController> logger, IGarageService garageService)
  {
    _logger = logger;
    _garageService = garageService;
  }

  [HttpPost("heatTimeRequest/save")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> SaveHeatTimeRequest(int id, HeatRequestDto request)
  {
    if (id < 1) throw new Exception("Such garage doesnt exists");
    await _garageService.SaveHeatTimeRequest(id, request);
    return this.NoContent();
  }

  [HttpGet("garages")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<IEnumerable<HeatRequestDto>>> GetGarages()
  {
    var garages = await _garageService.GetGarages();
    return this.Ok(garages);
  }

  [HttpGet("heatTimeRequest")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<IEnumerable<HeatRequestDto>>> GetHeatTimeRequests(int id)
  {
    if (id < 1) throw new Exception("Such garage doesnt exists");
    var heatTimeRequests = await _garageService.GetHeatTimeRequests(id);

    if (heatTimeRequests == null) throw new Exception("No heat requests available");
    return this.Ok(heatTimeRequests);
  }

  [HttpGet("Temperatures")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<List<OutsideTemperature>>> GetTemperatures(int id)
  {
    if (id < 1) throw new Exception("Such garage doesnt exists");
    var temperatures = await _garageService.GetTemperatures(id);

    if (!temperatures.Any()) throw new Exception("No temperatures stored");
    return this.Ok();
  }

  [HttpGet("CyclicHeatTimes")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<GarageCyclicHeatRequestsDto>> GetCyclicHeatTimes(int id)
  {
      if (id < 1) throw new Exception("Nie ma takiego garażu");

      var garageCyclicHeatRequests = await _garageService.GetCyclicHeatRequests(id);
  
      if (garageCyclicHeatRequests == null) throw new Exception("Nie ma danych w bazie danych");

      return this.Ok(garageCyclicHeatRequests);
  }
  
  [HttpPut("CyclicHeatTimes/save")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<ActionResult> UpdateCyclicHeatTimes(int id, CyclicHeatRequestsDto requestDto)
  {
    if (requestDto == null) throw new Exception("Brak danych do zapisania");
    if (id < 1) throw new Exception("Such garage doesnt exists");
    await _garageService.CreateOrUpdateCyclicHeatRequests(id, requestDto);

    return this.NoContent();
  }

  [HttpPatch("Heater")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult<string>> SetHeatingOn(int id)
  {
    var values = new Dictionary<string, string> { { "heat", "true" } };

    var content = new FormUrlEncodedContent(values);

    var garage = await _garageService.GetGarageById(id);

    if (garage == null) throw new Exception("This garage doesnt exist");
    var response = await GarageClient.ChangeHeaterStatus("ON");
    var responseString = await response.

    return this.Ok();
  }
}
