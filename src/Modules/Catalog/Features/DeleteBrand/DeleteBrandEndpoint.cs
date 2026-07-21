using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Catalog.Infrastructure;

namespace SPSApi.Modules.Catalog.Features.DeleteBrand;

public static class DeleteBrandEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapDelete("/api/catalog/brands/{id:int}", async (
      int id,
      [FromServices] CatalogDbContext db,
      CancellationToken ct
    ) =>
    {
      var brand = await db.Brands.FirstOrDefaultAsync(b => b.Id == id, ct);
      if (brand is null)
        return Results.NotFound(new { error = $"No existe una marca con Id {id}." });

      var inUse = await db.AssetModels.AnyAsync(m => m.BrandId == id, ct);
      if (inUse)
        return Results.Conflict(new
        {
          error = "No se puede eliminar la marca: hay modelos de equipo que lo usan. " +
            "Elimina o reasigna esos modelos primero."
        });

      db.Brands.Remove(brand);
      await db.SaveChangesAsync(ct);
      return Results.NoContent();
    }
    ).WithTags("Catalog").WithName("DeleteBrand");
  }
}
