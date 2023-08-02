using System.Threading.Tasks;
using SmartHome.TasksManager.Core.Entities;

namespace SmartHome.TasksManager.Core.Interfaces;

public interface IJsonFileReader
{
  Task<T> GetCyclicHeatDaysObject<T>(string filePath) where T: GaragesJsonObject;
}
