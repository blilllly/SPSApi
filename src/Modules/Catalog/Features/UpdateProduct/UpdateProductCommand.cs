using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Features.UpdateProduct;

public record UpdateProductCommand(
  int Id,
  string Name,
  ProductCategory Category,
  string UnitOfMeasure,
  string? Barcode,
  string? PartNumber,
  bool IsOriginal,
  decimal? EstimatedCost,
  int? EstimatedPageYield,
  bool IsActive
);
