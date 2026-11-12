using System;
using System.Collections.Generic;

namespace Core.Services;

/// <summary>
///   Heating state that must outlive a single <see cref="HeatingService" /> tick.
///   Registered as a singleton; <see cref="HeatingService" /> itself is scoped per tick.
///   Ticks run sequentially, so no synchronization is needed.
/// </summary>
public class HeatingState
{
  /// <summary>Last command successfully sent to each garage (used only to avoid duplicate HeatLog entries).</summary>
  public Dictionary<int, bool> LastCommanded { get; } = new();

  /// <summary>
  ///   End times of task occurrences that already started heating, per garage.
  ///   An occurrence stays ON until its end time even if the start-time calculation
  ///   later says "not yet" (temperature rose, or the reading is missing).
  /// </summary>
  private readonly Dictionary<int, HashSet<DateTime>> latchedEndTimes = new();

  public bool IsLatched(int garageId, DateTime endTime)
  {
    return latchedEndTimes.TryGetValue(garageId, out var ends) && ends.Contains(endTime);
  }

  public void Latch(int garageId, DateTime endTime)
  {
    if (!latchedEndTimes.TryGetValue(garageId, out var ends))
    {
      ends = [];
      latchedEndTimes[garageId] = ends;
    }

    ends.Add(endTime);
  }

  public void PruneLatches(DateTime now)
  {
    foreach (var ends in latchedEndTimes.Values)
    {
      ends.RemoveWhere(end => end <= now);
    }
  }
}
