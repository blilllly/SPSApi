using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Features.CreateProduct;

public record CreateProductCommand(
  string Sku,
  string Name,
  ProductCategory Category,
  string UnitOfMeasure,
  string? Barcode,
  decimal? EstimatedCost,
  int? EstimatedPageYield
);
