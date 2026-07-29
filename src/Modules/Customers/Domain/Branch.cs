namespace SPSApi.Modules.Customers.Domain;

public class Branch
{
  public int Id { get; private set; }
  public int CustomerId { get; set; }
  public Customer Customer { get; set; } = null!;
  public string Name { get; set; } = null!;
  public Address Address { get; set; } = null!;
  public bool IsActive { get; set; } = true;
}
