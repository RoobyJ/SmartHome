using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.TasksManager.Core.Interfaces;
using SmartHome.TasksManager.Core.Settings;

namespace SmartHome.TasksManager.Core.Services;

/// <summary>
///   An example service that performs business logic
/// </summary>
public class EntryPointService : IEntryPointService
{
  private readonly ILoggerAdapter<EntryPointService> _logger;
  private readonly IQueueReceiver _queueReceiver;
  private readonly IQueueSender _queueSender;
  private readonly IServiceLocator _serviceScopeFactoryLocator;
  private readonly EntryPointSettings _settings;

  public EntryPointService(ILoggerAdapter<EntryPointService> logger,
    EntryPointSettings settings,
    IQueueReceiver queueReceiver,
    IQueueSender queueSender,
    IServiceLocator serviceScopeFactoryLocator)
  {
    _logger = logger;
    _settings = settings;
    _queueReceiver = queueReceiver;
    _queueSender = queueSender;
    _serviceScopeFactoryLocator = serviceScopeFactoryLocator;
  }

  public async Task ExecuteAsync()
  {
    _logger.LogInformation("{service} running at: {time}", nameof(EntryPointService), DateTimeOffset.Now);
    try
    {
      // EF Requires a scope so we are creating one per execution here
      using var scope = _serviceScopeFactoryLocator.CreateScope();
      var repository =
        scope.ServiceProvider
          .GetService<IRepository>();

      // read from the queue
      var message = await _queueReceiver.GetMessageFromQueue(_settings.ReceivingQueueName);
      if (String.IsNullOrEmpty(message))
      {
        return;
      }
    }
#pragma warning disable CA1031 // Do not catch general exception types
    catch (Exception ex)
    {
      _logger.LogError(ex, $"{nameof(EntryPointService)}.{nameof(ExecuteAsync)} threw an exception.");
      // TODO: Decide if you want to re-throw which will crash the worker service
      //throw;
    }
#pragma warning restore CA1031 // Do not catch general exception types
  }
}
