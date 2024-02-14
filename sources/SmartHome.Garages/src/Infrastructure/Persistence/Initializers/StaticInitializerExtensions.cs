using Infrastructure.Persistence.Initializers.StaticInitializers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Initializers;

internal static class StaticInitializerExtensions
{
  public static void SeedWithStaticData(this ModelBuilder builder)
  {
    builder.SeedGarages();
    builder.SeedDaysInWeek();
  }
}
