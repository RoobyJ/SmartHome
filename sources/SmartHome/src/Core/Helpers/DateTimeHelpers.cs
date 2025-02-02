using System;

namespace Core.Helpers;

public static class DateTimeHelpers
{
  public static DateTime GetDateTimeFromWeekDayNumberAndTime(int weekDayNumber, TimeSpan time)
  {
    var currentDateTime = DateTime.Now;
    var currentWeekDay = (int)currentDateTime.DayOfWeek;
    if (currentWeekDay > weekDayNumber) currentDateTime = currentDateTime.AddDays((currentWeekDay - weekDayNumber)*(-1));
    if (currentWeekDay <= weekDayNumber) currentDateTime = currentDateTime.AddDays((weekDayNumber - currentWeekDay));
    currentDateTime = currentDateTime.Date + time;
    
    return currentDateTime;
  }
}
