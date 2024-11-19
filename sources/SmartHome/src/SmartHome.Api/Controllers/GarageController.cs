using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Mappers;
using Microsoft.AspNetCore.Mvc;

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

  [HttpGet("{id:int}/customHeatTasks")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<IEnumerable<HeatTaskDto>>> GetHeatTimeRequests(int id, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesn't exists");
    }

    var heatTimeRequests = await heatTaskService.GetHeatTimeTasks(id, ct);

    if (heatTimeRequests == null)
    {
      throw new Exception("No heat requests available");
    }

    return Ok(heatTimeRequests.Select(i => i.HeatTaskToDto()).ToList());
  }

  [HttpPost("{id:int}/customHeatTasks")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> SaveHeatTimeRequest(int id, CreateHeatTaskDto task, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesn't exists");
    }

    await heatTaskService.SaveHeatTimeTask(id, task, ct);
    return NoContent();
  }

  [HttpPut("{id:int}/customHeatTasks")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> UpdateHeatTimeRequest(int id, HeatTaskDto task, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesn't exists");
    }

    await heatTaskService.UpdateHeatTask(id, task, ct);
    return NoContent();
  }

  [HttpDelete("{id:int}/customHeatTasks")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> DeleteHeatTimeRequest(int id, int requestId, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesn't exists");
    }

    await heatTaskService.DeleteHeatTimeTask(id, requestId, ct);
    return NoContent();
  }

  [HttpGet("{id:int}/temperatures")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<List<OutsideTemperature>>> GetTemperatures(int id, [FromQuery] int days,
    CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesn't exists");
    }

    var temperatures = await garageService.GetTemperatures(id, days, ct);

    return Ok(temperatures);
  }

  [HttpGet("{id:int}/cyclicHeatTasks")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<ICollection<CyclicHeatTaskDto>>> GetCyclicHeatTimes(int id, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesn't exists");
    }

    var garageCyclicHeatTasks = await heatTaskService.GetCyclicHeatTasks(id, ct);
    
    return Ok(garageCyclicHeatTasks.Select(i => i.CyclicHeatTaskToDto()).ToList());
  }

  [HttpPost("{id:int}/cyclicHeatTasks")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> CreateCyclicHeatTimeRequest(int id, CreateCyclicHeatTaskDto taskDto,
    CancellationToken ct)
  {
    if (taskDto == null)
    {
      throw new Exception("No data to save");
    }

    if (id < 1)
    {
      throw new Exception("Such garage doesn't exists");
    }

    await heatTaskService.CreateCyclicHeatTask(id, taskDto, ct);

    return NoContent();
  }

  [HttpPut("{id:int}/cyclicHeatTasks")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> UpdateCyclicHeatTimeRequest(int id, UpdateCyclicHeatTaskDto requestDto,
    CancellationToken ct)
  {
    if (requestDto == null)
    {
      throw new Exception("No data to save");
    }

    if (id < 1)
    {
      throw new Exception("Such garage doesn't exists");
    }

    await heatTaskService.UpdateCyclicHeatTask(id, requestDto, ct);

    return NoContent();
  }

  [HttpDelete("{id:int}/cyclicHeatTasks")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult> DeleteCyclicHeatTimeRequest(int id, int requestId, CancellationToken ct)
  {
    if (id < 1)
    {
      throw new Exception("Such garage doesn't exists");
    }

    await heatTaskService.DeleteCyclicHeatTask(id, requestId, ct);
    return NoContent();
  }

  [HttpPatch("garage/{id:int}/heater")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult<string>> SetHeatingStatus(int id, [FromBody] bool newStatus, CancellationToken ct)
  {
    var garage = await garageService.GetGarageById(id, ct);

    if (garage == null)
    {
      throw new Exception("This garage doesn't exist");
    }

    var content = newStatus ? "ON" : "OFF";

    await garageClient.ChangeHeaterStatus(content, garage.Ip, ct);

    return NoContent();
  }

  [HttpGet("garage/heater-status")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<bool?>> GetGarageHeaterStatus(int garageId, CancellationToken cancellationToken)
  {
    var status = await garageService.GetGarageHeaterStatus(garageId, cancellationToken);
    return Ok(status);
  }
  
  [HttpPatch("garage/heat-tasks/active")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [Produces("application/json")]
  public async Task<ActionResult<bool?>> SetHeatTaskActive(SetHeatTaskActiveDto data, CancellationToken cancellationToken)
  {
    await heatTaskService.SetHeatTaskActive(data, cancellationToken);
    return Ok();
  }
}
