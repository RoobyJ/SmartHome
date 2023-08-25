using System.Threading.Tasks;
using SmartHome.Core.Entities;

namespace SmartHome.Core.Interfaces;

public interface IJsonFileReader
{
  Task<T> GetCyclicHeatDaysObject<T>(string filePath) where T: GaragesJsonObject;
}
