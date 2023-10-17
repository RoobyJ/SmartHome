using Microsoft.AspNetCore.Mvc;
using SmartHome.Core.Mappers;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;
using SmartHome.Core.Helpers;
using SmartHome.Core.Interfaces;

namespace SmartHome.api.Controllers;

[Route("api")]
public class GarageController : ApiControllerBase
{
  private readonly IGarageService _garageService;
  private readonly IHeatTaskService _heatTaskService;
  private readonly ILogger<GarageController> _logger;

  public GarageController(ILogger<GarageController> logger, IGarageService garageService, IHeatTaskService heatTaskService)
  {
    _logger = logger;
    _garageService = garageService;
    _heatTaskService = heatTaskService;
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

    var heatTimeRequests = await _heatTaskService.GetHeatTimeTasks(id, ct);

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

    await _heatTaskService.SaveHeatTimeTask(id, request, ct);
    return NoContent();
  }
  
  [HttpPut("{id:int}/heatTimeRequests")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> UpdateHeatTimeRequest(int id, HeatRequestDto request, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await _heatTaskService.SaveHeatTimeTask(id, request, ct);
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

    await _heatTaskService.DeleteHeatTimeTask(id, requestId, ct);
    return NoContent();
  }

  [HttpGet("{id:int}/Temperatures")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<List<OutsideTemperature>>> GetTemperatures(int id, [FromQuery]int days, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    var temperatures = await _garageService.GetTemperatures(id, days, ct);

    return Ok(temperatures);
  }

  [HttpGet("{id:int}/CyclicHeatTimes")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<ICollection<CyclicHeatTaskDto>>> GetCyclicHeatTimes(int id, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Nie ma takiego garażu");
    }

    var garageCyclicHeatTasks = await _heatTaskService.GetCyclicHeatTasks(id, ct);

    var result = garageCyclicHeatTasks.Select(GarageConverters.CyclicHeatTaskToDto);
    return Ok(result);
  }

  [HttpPost("{id:int}/CyclicHeatTimes")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> CreateCyclicHeatTimeRequest(int id, CreateCyclicHeatTaskDto taskDto, CancellationToken ct)
  {
    if (taskDto == null)
    {
      throw new Exception("Brak danych do zapisania");
    }

    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await _heatTaskService.CreateCyclicHeatTask(id, taskDto, ct);

    return NoContent();
  }
  
  [HttpPut("{id:int}/CyclicHeatTimes")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> UpdateCyclicHeatTimeRequest(int id, UpdateCyclicHeatTaskDto requestDto, CancellationToken ct)
  {
    if (requestDto == null)
    {
      throw new Exception("Brak danych do zapisania");
    }

    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await _heatTaskService.UpdateCyclicHeatTask(id, requestDto, ct);

    return NoContent();
  }
  
  [HttpDelete("{id:int}/CyclicHeatTimes")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> DeleteCyclicHeatTimeRequest(int id, int requestId, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await _heatTaskService.DeleteCyclicHeatTask(id, requestId, ct);
    return NoContent();
  }

  [HttpPatch("{id:int}/Heater")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult<string>> SetHeatingOn(int id, CancellationToken ct)
  {
    var garage = await _garageService.GetGarageById(id, ct);

    if (garage == null)
    {
      throw new Exception("This garage doesnt exist");
    }

    await GarageClient.ChangeHeaterStatus("ON", garage.Ip);

    return NoContent();
  }
}
