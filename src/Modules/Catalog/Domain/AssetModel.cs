namespace SPSApi.Modules.Catalog.Domain;

public class AssetModel
{
  public int Id { get; private set; }

  public int BrandId { get; set; }

  public Brand Brand { get; set; } = null!;

  public string Name { get; set; } = null!;

  public AssetType AssetType { get; set; }

  public string? PartNumber { get; set; }

  public bool IsColour { get; set; }
}
