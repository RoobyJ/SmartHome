using System.Threading.Tasks;

namespace SmartHome.Heater.Interfaces;

public interface IHeatingService
{
  Task ExecuteAsync();
}
