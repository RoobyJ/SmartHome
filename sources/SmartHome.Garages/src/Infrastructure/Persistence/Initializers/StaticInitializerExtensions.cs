using Microsoft.EntityFrameworkCore;
using SmartHome.Infrastructure.Persistence.Initializers.StaticInitializers;

namespace SmartHome.Infrastructure.Persistence.Initializers;

internal static class StaticInitializerExtensions
{
    public static void SeedWithStaticData(this ModelBuilder builder)
    {
      builder.SeedGarages();
      builder.SeedDaysInWeek();

    }
}
