using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Dtos;

public record ProductDto(
  int Id,
  string Sku,
  string Name,
  ProductCategory Category,
  string UnitOfMeasure,
  string? Barcode,
  bool IsActive,
  decimal? EstimatedCost,
  int? EstimatedPageYield
);
