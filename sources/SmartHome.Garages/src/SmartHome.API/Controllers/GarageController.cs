using Microsoft.AspNetCore.Mvc;
using SmartHome.Core.Common.Repositories;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;
using SmartHome.Core.Helpers;
using SmartHome.Core.Interfaces;
using SmartHome.webapi.Mappers;

namespace SmartHome.webapi.Controllers;

[Route("api")]
public class GarageController : ApiControllerBase
{
  private readonly IGarageService _garageService;
  private readonly IHeatRequestService _heatRequestService;
  private readonly ILogger<GarageController> _logger;

  public GarageController(ILogger<GarageController> logger, IGarageService garageService, IHeatRequestService heatRequestService)
  {
    _logger = logger;
    _garageService = garageService;
    _heatRequestService = heatRequestService;
  }

  [HttpGet("garages")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<ICollection<GarageDetailsDto>>> GetGarages(CancellationToken cancellationToken)
  {
    var garages = await _garageService.GetGarages(cancellationToken);
    return Ok(garages);
  }

  [HttpGet("{id:int}/heatTimeRequests")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<IEnumerable<HeatRequestDto>>> GetHeatTimeRequests(int id, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    var heatTimeRequests = await _heatRequestService.GetHeatTimeRequests(id, ct);

    if (heatTimeRequests == null)
    {
      throw new Exception("No heat requests available");
    }

    return Ok(heatTimeRequests);
  }
  
  [HttpPost("{id:int}/heatTimeRequests")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> SaveHeatTimeRequest(int id, HeatRequestDto request, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await _heatRequestService.SaveHeatTimeRequest(id, request, ct);
    return NoContent();
  }
  
  [HttpDelete("{id:int}/heatTimeRequests")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> DeleteHeatTimeRequest(int id, int requestId, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await _heatRequestService.DeleteHeatTimeRequest(id, requestId, ct);
    return NoContent();
  }

  [HttpGet("{id:int}/Temperatures")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<List<OutsideTemperature>>> GetTemperatures(int id, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    var temperatures = await _garageService.GetTemperatures(id, ct);

    return Ok(temperatures);
  }

  [HttpGet("{id:int}/CyclicHeatTimes")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<GarageCyclicHeatRequestsDto>> GetCyclicHeatTimes(int id, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Nie ma takiego garażu");
    }

    var garageCyclicHeatRequests = await _heatRequestService.GetCyclicHeatRequests(id, ct);

    return Ok(garageCyclicHeatRequests);
  }

  [HttpPost("{id:int}/CyclicHeatTimes")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<ActionResult> CreateCyclicHeatTimeRequest(int id, CyclicHeatRequestsDto requestDto, CancellationToken ct)
  {
    if (requestDto == null)
    {
      throw new Exception("Brak danych do zapisania");
    }

    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await _heatRequestService.CreateCyclicHeatRequest(id, requestDto, ct);

    return NoContent();
  }
  
  [HttpPut("{id:int}/CyclicHeatTimes")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<ActionResult> UpdateCyclicHeatTimeRequest(int id, CyclicHeatRequestsDto requestDto, CancellationToken ct)
  {
    if (requestDto == null)
    {
      throw new Exception("Brak danych do zapisania");
    }

    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await _heatRequestService.UpdateCyclicHeatRequest(id, requestDto, ct);

    return NoContent();
  }

  [HttpPatch("{id:int}/Heater")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult<string>> SetHeatingOn(int id, CancellationToken ct)
  {
    var values = new Dictionary<string, string> { { "heat", "true" } };

    var content = new FormUrlEncodedContent(values);

    var garage = await _garageService.GetGarageById(id, ct);

    if (garage == null)
    {
      throw new Exception("This garage doesnt exist");
    }

    await GarageClient.ChangeHeaterStatus("ON", garage.Ip);

    return Ok();
  }
}
