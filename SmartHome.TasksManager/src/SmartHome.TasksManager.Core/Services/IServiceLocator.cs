using System;
using Microsoft.Extensions.DependencyInjection;

namespace SmartHome.TasksManager.Core.Services;

public interface IServiceLocator : IDisposable
{
  IServiceScope CreateScope();
  T Get<T>();
}
