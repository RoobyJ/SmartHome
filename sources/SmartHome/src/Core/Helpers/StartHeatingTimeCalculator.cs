using System;

namespace Core.Helpers;

public static class StartHeatingTimeCalculator
{
  /// <summary>Upper bound on pre-heating; the formula gives about 3 h at -40 C.</summary>
  public static readonly TimeSpan MaxLeadTime = TimeSpan.FromHours(4);

  /// <summary>Plausible garage temperature range. Readings outside it are sensor faults
  /// (DS18B20 reports 85 C on power-on and -127 C when disconnected).</summary>
  public const float MinPlausibleTemperature = -40;

  public const float MaxPlausibleTemperature = 60;

  public static bool IsPlausible(float temperature)
  {
    return temperature is >= MinPlausibleTemperature and <= MaxPlausibleTemperature;
  }

  /// <summary>
  ///   Returns when heating must start to finish at <paramref name="endHeatTime" />,
  ///   or null when the temperature is unknown or not a number.
  ///   The lead time is clamped to [0, <see cref="MaxLeadTime" />].
  /// </summary>
  public static DateTime? CalculateStartTime(DateTime endHeatTime, float? temperature)
  {
    if (temperature == null || !float.IsFinite(temperature.Value)) return null;

    var leadMinutes = Math.Clamp(CalculateOnHeatTime(temperature.Value), 0, MaxLeadTime.TotalMinutes);
    return endHeatTime - TimeSpan.FromMinutes(leadMinutes);
  }

  #region private methods

  /// <summary>Pre-heating time in minutes for a given temperature. Negative above about 71 C.</summary>
  private static double CalculateOnHeatTime(float temp)
  {
    return 10 * (5 * (1 + 5 * Math.Pow(Math.E, -0.02 * temp)) - 11) * 0.37;
  }

  #endregion
}
