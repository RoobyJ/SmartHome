using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Core.Entities;
using SmartHome.Core.Models;

namespace SmartHome.Core.Interfaces;

public interface IHeatingService
{
  Task ExecuteAsync();
  
}
