namespace SPSApi.Modules.Customers.Domain;

public class Address
{
  public string MainStreet { get; set; } = null!;
  public string? SecondaryStreet { get; set; }
  public string? BuildingNumber { get; set; }
  public string? City { get; set; }
  public string Province { get; set; } = null!;
  public string? PostalCode { get; set; }
  public string? Reference { get; set; }
  public decimal? Latitude { get; set; }
  public decimal? Longitude { get; set; }
}
