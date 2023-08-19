using System.Text.Json;
using System.Threading.Tasks;
using SmartHome.TasksManager.Core.Entities;
using SmartHome.TasksManager.Core.Interfaces;


namespace SmartHome.TasksManager.Infrastructure.Data;

public class JsonFileReader : IJsonFileReader
{
  public async Task<T> GetCyclicHeatDaysObject<T>(string filePath) where T : GaragesJsonObject
  {
    using var stream = System.IO.File.OpenRead(filePath);
    return await  JsonSerializer.DeserializeAsync<T>(stream);
  }
  
}
