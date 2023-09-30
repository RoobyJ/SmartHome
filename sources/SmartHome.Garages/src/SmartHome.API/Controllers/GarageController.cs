using Microsoft.AspNetCore.Mvc;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;
using SmartHome.Core.Helpers;
using SmartHome.Core.Interfaces;
using SmartHome.webapi.Mappers;

namespace SmartHome.webapi.Controllers;

[Route("garage/{id:int}")]
public class GarageController : ApiControllerBase
{
  private readonly IGarageService _garageService;
  private readonly ILogger<GarageController> _logger;

  public GarageController(ILogger<GarageController> logger, IGarageService garageService)
  {
    _logger = logger;
    _garageService = garageService;
  }

  [HttpPost("heatTimeRequest/save")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> SaveHeatTimeRequest(int id, HeatRequestDto request)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await _garageService.SaveHeatTimeRequest(id, request);
    return NoContent();
  }

  [HttpGet("garages")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<ICollection<GarageDetailsDto>>> GetGarages(int id)
  {
    var garages = await _garageService.GetGarages();
    var result = new List<GarageDetailsDto>();
    
    foreach (var garage in garages)
    {
      var response = await GarageClient.GetHeaterStatus(garage.Ip);
      result.Add(GarageConverters.GarageToGarageDetailsDto(garage, response));
    }
    
    return Ok(result);
  }

  [HttpGet("heatTimeRequest")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<IEnumerable<HeatRequestDto>>> GetHeatTimeRequests(int id)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    var heatTimeRequests = await _garageService.GetHeatTimeRequests(id);

    if (heatTimeRequests == null)
    {
      throw new Exception("No heat requests available");
    }

    return Ok(heatTimeRequests);
  }

  [HttpGet("Temperatures")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<List<OutsideTemperature>>> GetTemperatures(int id)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    var temperatures = await _garageService.GetTemperatures(id);

    if (!temperatures.Any())
    {
      throw new Exception("No temperatures stored");
    }

    return Ok();
  }

  [HttpGet("CyclicHeatTimes")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<GarageCyclicHeatRequestsDto>> GetCyclicHeatTimes(int id)
  {
    if (id < 1)
    {
      throw new Exception("Nie ma takiego garażu");
    }

    var garageCyclicHeatRequests = await _garageService.GetCyclicHeatRequests(id);

    if (garageCyclicHeatRequests == null)
    {
      throw new Exception("Nie ma danych w bazie danych");
    }

    return Ok(garageCyclicHeatRequests);
  }

  [HttpPut("CyclicHeatTimes/save")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<ActionResult> UpdateCyclicHeatTimes(int id, CyclicHeatRequestsDto requestDto)
  {
    if (requestDto == null)
    {
      throw new Exception("Brak danych do zapisania");
    }

    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await _garageService.CreateOrUpdateCyclicHeatRequests(id, requestDto);

    return NoContent();
  }

  [HttpPatch("Heater")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult<string>> SetHeatingOn(int id)
  {
    var values = new Dictionary<string, string> { { "heat", "true" } };

    var content = new FormUrlEncodedContent(values);

    var garage = await _garageService.GetGarageById(id);

    if (garage == null)
    {
      throw new Exception("This garage doesnt exist");
    }

    await GarageClient.ChangeHeaterStatus("ON", garage.Ip);

    return Ok();
  }
}
