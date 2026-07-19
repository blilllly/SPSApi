using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Features.ListProducts;

public record ListProductsQuery(
  ProductCategory? Category = null,
  string? Search = null,
  bool IncludeInactive = false
);
