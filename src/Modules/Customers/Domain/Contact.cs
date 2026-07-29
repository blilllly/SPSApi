namespace SPSApi.Modules.Customers.Domain;

public class Contact
{
  public int Id { get; private set; }
  public int CustomerId { get; set; }
  public Customer Customer { get; set; } = null!;
  public int? BranchId { get; set; }
  public Branch? Branch { get; set; }
  public string Name { get; set; } = null!;
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public string? Position { get; set; }
}
