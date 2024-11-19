namespace Core.Dtos;

public class SetHeatTaskActiveDto
{
  public int Id { get; set; }
  public bool Active { get; set; }
  public bool IsCyclic { get; set; }
}
