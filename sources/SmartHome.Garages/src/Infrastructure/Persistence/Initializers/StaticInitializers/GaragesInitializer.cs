﻿using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Persistence.Initializers.StaticInitializers;

internal static class GaragesInitializer
{
  public static ModelBuilder SeedGarages(this ModelBuilder builder)
  {
    builder.Entity<Garage>().HasData(Garages.GarageRobert);

    return builder;
  }

  #region Nested types

  private static class Garages
  {
    public static Garage GarageRobert => new() { Id = 1, Name = "Garage Robert", Ip = "192.168.1.10" };
  }

  #endregion
}
