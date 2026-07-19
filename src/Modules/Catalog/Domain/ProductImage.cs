namespace SPSApi.Modules.Catalog.Domain;

public class ProductImage
{
  public int Id { get; private set; }

  public int ProductId { get; set; }

  public Product Product { get; set; } = null!;

  public string Url { get; set; } = null!;

  public int SortOrder { get; set; }

  public bool IsPrimary { get; set; }
}
