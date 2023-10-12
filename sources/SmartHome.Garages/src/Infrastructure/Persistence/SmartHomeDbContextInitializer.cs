using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Infrastructure.Data;

namespace SmartHome.Infrastructure.Persistence;

internal class SmartHomeDbContextInitializer
{
    private readonly SmartHomeDbContext dbContext;
    private readonly ILogger<SmartHomeDbContextInitializer> logger;


    public SmartHomeDbContextInitializer(SmartHomeDbContext dbContext,
                                           ILogger<SmartHomeDbContextInitializer> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // this will create the database, run all migrations (including the static seeds)
            if (this.dbContext.Database.IsNpgsql()) await this.dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }
}
