using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Dtos;

public record ProductDto(
  int Id,
  string Sku,
  string Name,
  ProductCategory Category,
  string UnitOfMeasure,
  string? Barcode,
  string? PartNumber,
  bool IsOriginal,
  bool IsActive,
  decimal? EstimatedCost,
  int? EstimatedPageYield,
  IReadOnlyList<ProductImageDto> Images
  );

public record ProductImageDto(
  int Id,
  string Url,
  int SortOrder,
  bool IsPrimary
);