using System;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Catalog.Infrastructure;
using SPSApi.Shared.Primitives;

namespace SPSApi.Modules.Catalog.Features.DeleteProduct;

public class DeleteProductHandler(CatalogDbContext db)
{
  public async Task<Result<bool>> HandleAsync(int id, CancellationToken ct)
  {
    var product = await db.Products
      .Include(p => p.Images)
      .FirstOrDefaultAsync(p => p.Id == id, ct);

    if (product is null)
      return Result<bool>.Failure($"No existe un producto con Id {id}.");

    // TODO(Inventory): cuando exista el módulo Inventory, verificar aquí que el
    // producto NO tenga StockMovements. Si los tiene, NO borrar físicamente —
    // devolver un error que sugiera desactivarlo (soft delete vía IsActive), para
    // no romper la trazabilidad del kardex ni los reportes de rentabilidad.
    // Por ahora Inventory no existe, así que ningún producto tiene movimientos y
    // el borrado físico es seguro.

    // Las imágenes se borran en cascada (FK Cascade), pero al estar cargadas
    // EF las elimina explícitamente también. Coherente.
    db.Products.Remove(product);
    await db.SaveChangesAsync(ct);

    return Result<bool>.Success(true);
  }
}
