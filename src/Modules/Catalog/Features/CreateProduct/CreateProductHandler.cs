using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Catalog.Domain;
using SPSApi.Modules.Catalog.Dtos;
using SPSApi.Modules.Catalog.Infrastructure;
using SPSApi.Shared.Primitives;

namespace SPSApi.Modules.Catalog.Features.CreateProduct;

public class CreateProductHandler(CatalogDbContext db)
{
  public async Task<Result<ProductDto>> HandleAsync(CreateProductCommand cmd, CancellationToken ct)
  {
    var skuTaken = await db.Products.AnyAsync(p => p.Sku == cmd.Sku, ct);
    if (skuTaken)
      return Result<ProductDto>.Failure($"Ya existe un producto con el SKU '{cmd.Sku}'");

    var product = new Product
    {
      Sku = cmd.Sku,
      Name = cmd.Name,
      Category = cmd.Category,
      UnitOfMeasure = cmd.UnitOfMeasure,
      Barcode = cmd.Barcode,
      EstimatedCost = cmd.EstimatedCost,
      EstimatedPageYield = cmd.EstimatedPageYield,
      IsActive = true
    };

    db.Products.Add(product);
    await db.SaveChangesAsync(ct);

    return Result<ProductDto>.Success(new ProductDto(
      product.Id, product.Sku, product.Name, product.Category,
      product.UnitOfMeasure, product.Barcode, product.IsActive,
      product.EstimatedCost, product.EstimatedPageYield, []
    ));
  }
}
