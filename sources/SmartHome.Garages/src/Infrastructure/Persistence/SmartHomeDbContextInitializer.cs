﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

internal class SmartHomeDbContextInitializer(
  SmartHomeDbContext dbContext,
  ILogger<SmartHomeDbContextInitializer> logger)
{
  public async Task InitializeAsync()
  {
    try
    {
      // this will create the database, run all migrations (including the static seeds)
      if (dbContext.Database.IsNpgsql())
      {
        await dbContext.Database.MigrateAsync();
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "An error occurred while initializing the database");
      throw;
    }
  }
}
