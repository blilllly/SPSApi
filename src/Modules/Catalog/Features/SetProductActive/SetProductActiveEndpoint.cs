using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Catalog.Infrastructure;

namespace SPSApi.Modules.Catalog.Features.SetProductActive;

public static class SetProductActiveEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPatch("/api/catalog/products/{id:int}", async (
      int id,
      SetActiveBody body,
      [FromServices] CatalogDbContext db,
      CancellationToken ct
    ) =>
    {
      var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
      if (product is null)
        return Results.NotFound(new { error = $"No existe un producto con Id {id}." });

      product.IsActive = body.IsActive;
      await db.SaveChangesAsync(ct);

      return Results.Ok(new { product.Id, product.Sku, product.IsActive });
    }
    ).WithTags("Catalog").WithName("SetProductActive");
  }
}

public record SetActiveBody(bool IsActive);