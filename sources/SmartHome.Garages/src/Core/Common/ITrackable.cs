using System;

namespace SmartHome.Core.Common;

public interface ITrackable
{
  public DateTimeOffset Created { get; set; }

  public int? CreatedBy { get; set; }

  public DateTimeOffset? LastModified { get; set; }

  public int? LastModifiedBy { get; set; }
}
