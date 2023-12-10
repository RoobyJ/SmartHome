using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities;

namespace SmartHome.Infrastructure.Persistence.Initializers.StaticInitializers;

internal static class DaysInWeekInitializer
{
  public static ModelBuilder SeedDaysInWeek(this ModelBuilder builder)
  {
    builder.Entity<DayInWeek>().HasData(DaysInWeek.Sunday);
    builder.Entity<DayInWeek>().HasData(DaysInWeek.Monday);
    builder.Entity<DayInWeek>().HasData(DaysInWeek.Tuesday);
    builder.Entity<DayInWeek>().HasData(DaysInWeek.Wednesday);
    builder.Entity<DayInWeek>().HasData(DaysInWeek.Thursday);
    builder.Entity<DayInWeek>().HasData(DaysInWeek.Friday);
    builder.Entity<DayInWeek>().HasData(DaysInWeek.Saturday);

    return builder;
  }

  #region Nested types

  internal static class DaysInWeek
  {
    public static DayInWeek Sunday => new() { Id = 1, Name = "Sunday" };
    public static DayInWeek Monday => new() { Id = 2, Name = "Monday" };
    public static DayInWeek Tuesday => new() { Id = 3, Name = "Tuesday" };
    public static DayInWeek Wednesday => new() { Id = 4, Name = "Wednesday" };
    public static DayInWeek Thursday => new() { Id = 5, Name = "Thursday" };
    public static DayInWeek Friday => new() { Id = 6, Name = "Friday" };
    public static DayInWeek Saturday => new() { Id = 7, Name = "Saturday" };
  }

  #endregion
}
