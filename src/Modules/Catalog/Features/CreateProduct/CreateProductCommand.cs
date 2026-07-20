using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Features.CreateProduct;

public record CreateProductCommand(
  string Sku,
  string Name,
  ProductCategory Category,
  string UnitOfMeasure,
  string? Barcode,
  string? PartNumber,
  bool IsOriginal,
  decimal? EstimatedCost,
  int? EstimatedPageYield
);
