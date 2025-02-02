using System;
using Core.Interfaces;

namespace Core.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime Today => DateTime.Today;
}
