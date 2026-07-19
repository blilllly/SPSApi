namespace SPSApi.Modules.Catalog.Domain;

public class Product
{
  public int Id { get; private set; }

  public string Sku { get; set; } = null!;

  public string Name { get; set; } = null!;

  public ProductCategory Category { get; set; }

  public string UnitOfMeasure { get; set; } = "EA";

  public string? Barcode { get; set; }

  public bool IsActive { get; set; } = true;

  public decimal? EstimatedCost { get; set; }

  public int? EstimatedPageYield { get; set; }

  public ICollection<ProductImage> Images { get; private set; } = [];
}
