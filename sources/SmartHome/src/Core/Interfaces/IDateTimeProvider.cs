using System;

namespace Core.Interfaces;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime Today { get; }
}
