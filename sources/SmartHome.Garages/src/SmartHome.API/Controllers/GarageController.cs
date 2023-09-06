using Microsoft.AspNetCore.Mvc;
using SmartHome.webapi.Models;
using SmartHome.Core.Entities;
using System.Text.Json;
using SmartHome.Core.Common.Repositories;

namespace SmartHome.webapi.Controllers;

[Route("garage/{id:int}")]
public class GarageController : ApiControllerBase
{
    private readonly ILogger<GarageController> _logger;
    private readonly IGarageRepository _context;
    
    private static readonly HttpClient Client = new HttpClient();

    public GarageController(ILogger<GarageController> logger, IGarageRepository context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost("heatTimeRequest/save")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SaveHeatTimeRequest(int id, HeatRequestDto request)
    {
        
        // _context.Add<HeatRequest>(new HeatRequest() {HeatRequest1 = request.Time, GarageId = id});
        // await _context.SaveChangesAsync();
        await Task.Delay(1);
        return this.NoContent();
    }

    [HttpGet("heatTimeRequest")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<ActionResult<IEnumerable<HeatRequestDto>>> GetHeatTimeRequests(int id)
    {
        //var list = await _context.HeatRequests.Where(item => item.GarageId == id).ToListAsync();
        // TODO: Filters to implement
        //if (!list.Any()) throw new Exception("Nie ma niczego w bazie");
        await Task.Delay(1);
        return this.Ok();
    }

    [HttpGet("Temperatures")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<ActionResult<List<OutsideTemperature>>> GetTemperatures(int id)
    {
        //var listWithTemperatures = await _context.OutsideTemperatures.Where(item => item.GarageId == id).ToListAsync();

        //if (!listWithTemperatures.Any()) throw new Exception("Nie ma danych o temperaturze w bazie danych");
        await Task.Delay(1);
        return this.Ok();
    }

    // [HttpGet("CyclicHeatTimes")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [Produces("application/json")]
    // public async Task<ActionResult<CyclicHeatDays>> GetCyclicHeatTimes(int id)
    // {
    //     if (id < 1) throw new Exception("Nie ma takiego garażu");
    //     
    //     var data = await JsonFile.ReadAsync<GaragesJsonObject>(@"../../CyclicHeatDays.json");
    //
    //     if (data == null) throw new Exception("Nie ma danych w bazie danych");
    //     
    //     if (data.Garages.Count < id) throw new Exception("Nie ma takiego garażu");
    //
    //     return this.Ok(data.Garages[id]);
    // }
    //
    // [HttpPatch("CyclicHeatTimes/save")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // public async Task<ActionResult> UpdateCyclicHeatTimes(int id, CyclicHeatDays request)
    // {
    //     if (request == null) throw new Exception("Brak danych do zapisania");
    //     if (id < 1) throw new Exception("Nie ma takiego garażu");
    //     
    //     var data = JsonFile.ReadAsync<GaragesJsonObject>(@"../CyclicHeatDays.json");
    //     
    //     if (data.Result == null) throw new Exception("Nie ma danych w bazie danych");
    //     if (data.Result.Garages.Count < id) throw new Exception("Nie ma takiego garażu");
    //
    //     data.Result.Garages[id] = request;
    //     await JsonFile.SaveAsync(@"../../CyclicHeatDays.json", data.Result.Garages);
    //
    //     return this.NoContent();
    // }
    
    [HttpPatch("Heater")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<string>> SetHeatingOn(int id)
    {
        
        
        var values = new Dictionary<string, string>
        {
            { "heat", "true" }
        };

        var content = new FormUrlEncodedContent(values);
        
        //var garage = await _context.Garages.Where(item => item.Id == id).SingleOrDefaultAsync();

        //if (garage == null) throw new Exception("Nie ma danych o temperaturze w bazie danych");
        
        //var response = await Client.PutAsync($"http://{garage.Ip}/Heater/", content);

        //var responseString = await response.Content.ReadAsStringAsync();
        await Task.Delay(1);

        return this.Ok();
    }

    #region private
    private static class JsonFile
    {
        public static async Task<T?> ReadAsync<T>(string filePath)
        {
            await using var stream = System.IO.File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
        
        public static async Task SaveAsync<T>(string filePath, T data)
        {
            var json = JsonSerializer.Serialize(data);
            await System.IO.File.WriteAllTextAsync(filePath, json);
        }
    }
    #endregion
}

