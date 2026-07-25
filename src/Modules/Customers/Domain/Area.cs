namespace SPSApi.Modules.Customers.Domain;

public class Area
{
  public int Id { get; private set; }
  public int BranchId { get; set; }
  public Branch Branch { get; set; } = null!;
  public string Name { get; set; } = null!;
}
