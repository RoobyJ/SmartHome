using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Core.Dtos;
using SmartHome.Core.Entities;
using SmartHome.Core.Interfaces;
using SmartHome.Core.Mappers;

namespace SmartHome.Api.Controllers;

[Route("api")]
public class GarageController(
  IGarageService garageService,
  IHeatTaskService heatTaskService,
  IGarageClient garageClient)
  : ApiControllerBase
{
  [HttpGet("garages")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<ICollection<GarageDetailsDto>>> GetGarages(CancellationToken cancellationToken)
  {
    var garages = await garageService.GetGarages(cancellationToken);
    return Ok(garages);
  }

  [HttpGet("{id:int}/heatTimeRequests")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<IEnumerable<HeatTaskDto>>> GetHeatTimeRequests(int id, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    var heatTimeRequests = await heatTaskService.GetHeatTimeTasks(id, ct);

    if (heatTimeRequests == null)
    {
      throw new Exception("No heat requests available");
    }

    return Ok(heatTimeRequests);
  }

  [HttpPost("{id:int}/heatTimeRequests")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> SaveHeatTimeRequest(int id, CreateHeatTaskDto task, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await heatTaskService.SaveHeatTimeTask(id, task, ct);
    return NoContent();
  }

  [HttpPut("{id:int}/heatTimeRequests")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> UpdateHeatTimeRequest(int id, HeatTaskDto task, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await heatTaskService.UpdateHeatTask(id, task, ct);
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

    await heatTaskService.DeleteHeatTimeTask(id, requestId, ct);
    return NoContent();
  }

  [HttpGet("{id:int}/Temperatures")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<List<OutsideTemperature>>> GetTemperatures(int id, [FromQuery] int days,
    CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    var temperatures = await garageService.GetTemperatures(id, days, ct);

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

    var garageCyclicHeatTasks = await heatTaskService.GetCyclicHeatTasks(id, ct);

    var result = garageCyclicHeatTasks.Select(GarageConverters.CyclicHeatTaskToDto);
    return Ok(result);
  }

  [HttpPost("{id:int}/CyclicHeatTimes")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> CreateCyclicHeatTimeRequest(int id, CreateCyclicHeatTaskDto taskDto,
    CancellationToken ct)
  {
    if (taskDto == null)
    {
      throw new Exception("Brak danych do zapisania");
    }

    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await heatTaskService.CreateCyclicHeatTask(id, taskDto, ct);

    return NoContent();
  }

  [HttpPut("{id:int}/CyclicHeatTimes")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> UpdateCyclicHeatTimeRequest(int id, UpdateCyclicHeatTaskDto requestDto,
    CancellationToken ct)
  {
    if (requestDto == null)
    {
      throw new Exception("Brak danych do zapisania");
    }

    if (id < 1)
    {
      throw new Exception("Such garage doesnt exists");
    }

    await heatTaskService.UpdateCyclicHeatTask(id, requestDto, ct);

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

    await heatTaskService.DeleteCyclicHeatTask(id, requestId, ct);
    return NoContent();
  }

  [HttpPatch("{id:int}/Heater")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult<string>> SetHeatingOn(int id, CancellationToken ct)
  {
    var garage = await garageService.GetGarageById(id, ct);

    if (garage == null)
    {
      throw new Exception("This garage doesnt exist");
    }

    await garageClient.ChangeHeaterStatus("ON", garage.Ip, ct);

    return NoContent();
  }
}
