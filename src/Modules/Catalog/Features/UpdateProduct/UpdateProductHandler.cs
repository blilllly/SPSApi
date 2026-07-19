using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Catalog.Dtos;
using SPSApi.Modules.Catalog.Infrastructure;
using SPSApi.Shared.Primitives;

namespace SPSApi.Modules.Catalog.Features.UpdateProduct;

public class UpdateProductHandler(CatalogDbContext db)
{
  public async Task<Result<ProductDto>> HandleAsync(
    UpdateProductCommand cmd,
    CancellationToken ct)
  {
    var product = await db.Products.FirstOrDefaultAsync(p => p.Id == cmd.Id, ct);

    if (product is null)
      return Result<ProductDto>.Failure($"No existe un producto con Id {cmd.Id}.");

    // El Sku no es editable
    product.Name = cmd.Name;
    product.Category = cmd.Category;
    product.UnitOfMeasure = cmd.UnitOfMeasure;
    product.Barcode = cmd.Barcode;
    product.EstimatedCost = cmd.EstimatedCost;
    product.EstimatedPageYield = cmd.EstimatedPageYield;
    product.IsActive = cmd.IsActive;

    await db.SaveChangesAsync(ct);

    return Result<ProductDto>.Success(new ProductDto(
      product.Id, product.Sku, product.Name, product.Category,
      product.UnitOfMeasure, product.Barcode, product.IsActive,
      product.EstimatedCost, product.EstimatedPageYield,
      product.Images
        .OrderBy(i => i.SortOrder)
        .Select(i => new ProductImageDto(i.Id, i.Url, i.SortOrder, i.IsPrimary))
        .ToList()
    ));
  }
}
