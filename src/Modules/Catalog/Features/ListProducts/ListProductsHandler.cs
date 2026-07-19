using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Catalog.Dtos;
using SPSApi.Modules.Catalog.Infrastructure;

namespace SPSApi.Modules.Catalog.Features.ListProducts;

public class ListProductsHandler(CatalogDbContext db)
{

  public async Task<IReadOnlyList<ProductDto>> HandleAsync(ListProductsQuery q, CancellationToken ct)
  {
    var query = db.Products.AsNoTracking().AsQueryable();

    if (!q.IncludeInactive)
      query = query.Where(p => p.IsActive);

    if (q.Category is not null)
      query = query.Where(p => p.Category == q.Category);

    if (!string.IsNullOrWhiteSpace(q.Search))
    {
      var term = q.Search.Trim();
      query = query.Where(p => p.Name.Contains(term) || p.Sku.Contains(term));
    }

    return await query
      .OrderBy(p => p.Name)
      .Select(p => new ProductDto(
          p.Id, p.Sku, p.Name, p.Category,
          p.UnitOfMeasure, p.Barcode, p.IsActive,
          p.EstimatedCost, p.EstimatedPageYield,
          p.Images
            .OrderBy(i => i.SortOrder)
            .Select(i => new ProductImageDto(i.Id, i.Url, i.SortOrder, i.IsPrimary))
            .ToList()
        )
      ).ToListAsync(ct);
  }

}
