using System.Collections.Specialized;
using Microsoft.AspNetCore.Mvc;
using SmartHome.webapi.Models;
using SmartHome.webapi.Entities;
using Microsoft.EntityFrameworkCore;
using SmartHome.webapi.Context;
using System.Text.Json;
using System.IO;
using System.Net;
using System.Text;

namespace SmartHome.webapi.Controllers;

[Route("garage/{id:int}")]
public class GarageController : ApiControllerBase
{
    private readonly ILogger<GarageController> _logger;
    
    private static readonly HttpClient client = new HttpClient();

    public GarageController(ILogger<GarageController> logger)
    {
        _logger = logger;
    }

    [HttpPost("heatTimeRequest/save")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SaveHeatTimeRequest(int id, HeatRequestDto request)
    {
        await using (var context = new SmartHomeDBContext())
        {
            await context.Database.EnsureCreatedAsync();

            context.Add(request);
            await context.SaveChangesAsync();
        }

        return this.NoContent();
    }

    [HttpGet("heatTimeRequest")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<ActionResult<IEnumerable<HeatRequestDto>>> GetHeatTimeRequests(int id)
    {
        await using var context = new SmartHomeDBContext();
        var list = await context.HeatRequests.Where(item => item.garageId == id).ToListAsync();

        if (!list.Any()) throw new Exception("Nie ma niczego w bazie");

        return this.Ok(list);
    }

    [HttpGet("Temperatures")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<ActionResult<List<OutsideTemperature>>> GetTemperatures(int id)
    {
        await using var context = new SmartHomeDBContext();
        var listWithTemperatures = await context.OutsideTemperatures.Where(item => item.garageId == id).ToListAsync();

        if (!listWithTemperatures.Any()) throw new Exception("Nie ma danych o temperaturze w bazie danych");

        return this.Ok(listWithTemperatures);
    }

    [HttpGet("CyclicHeatTimes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<ActionResult<CyclicHeatDays>> GetCyclicHeatTimes(int id)
    {
        if (id < 1) throw new Exception("Nie ma takiego garażu");
        
        var data = JsonFile.ReadAsync<Garages>(@"../../CyclicHeatDays.json");
        
        if (data.Result == null) throw new Exception("Nie ma danych w bazie danych");
        
        if (data.Result.Items.Length < id) throw new Exception("Nie ma takiego garażu");

        return this.Ok(data.Result.Items[id]);
    }

    [HttpPatch("CyclicHeatTimes/save")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateCyclicHeatTimes(int id, CyclicHeatDays request)
    {
        if (request == null) throw new Exception("Brak danych do zapisania");
        if (id < 1) throw new Exception("Nie ma takiego garażu");
        
        var data = JsonFile.ReadAsync<Garages>(@"../../CyclicHeatDays.json");
        
        if (data.Result == null) throw new Exception("Nie ma danych w bazie danych");
        if (data.Result.Items.Length < id) throw new Exception("Nie ma takiego garażu");

        data.Result.Items[id] = request;
        await JsonFile.SaveAsync(@"../../CyclicHeatDays.json", data.Result.Items);

        return this.NoContent();
    }
    
    [HttpPatch("Heater")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<string>> SetHeatingOn(int id)
    {
        
        
        var values = new Dictionary<string, string>
        {
            { "heat", "true" }
        };

        var content = new FormUrlEncodedContent(values);

        var response = await client.PutAsync("http://www.example.com/recepticle.aspx", content);

        var responseString = await response.Content.ReadAsStringAsync();

        return this.Ok(responseString);
    }

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
}

