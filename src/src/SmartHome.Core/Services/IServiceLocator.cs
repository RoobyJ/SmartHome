using System;
using Microsoft.Extensions.DependencyInjection;

namespace SmartHome.Core.Services;

public interface IServiceLocator : IDisposable
{
  IServiceScope CreateScope();
  T Get<T>();
}
