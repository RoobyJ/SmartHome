using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Initializers.StaticInitializers;

internal static class GaragesInitializer
{
  public static ModelBuilder SeedGarages(this ModelBuilder builder)
  {
    builder.Entity<GarageEntity>().HasData(Garages.GarageEntityRobert);

    return builder;
  }

  #region Nested types

  private static class Garages
  {
    public static GarageEntity GarageEntityRobert => new() { Id = 1, Name = "Garage Robert", Ip = "192.168.5.142" };
  }

  #endregion
}
