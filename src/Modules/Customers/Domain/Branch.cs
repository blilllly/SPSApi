namespace SPSApi.Modules.Customers.Domain;

public class Branch
{
  public int Id { get; private set; }
  public int CustomerId { get; set; }
  public Customer Customer { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string? Address { get; set; }
  public decimal? Latitude { get; set; }
  public decimal? Longitude { get; set; }
  public bool IsActive { get; set; } = true;
}
