namespace SPSApi.Modules.Customers.Domain;

public class Customer
{
  public int Id { get; private set; }
  public string Name { get; set; } = null!;
  public string? TaxId { get; set; }
  public bool IsActive { get; set; } = true;
}