using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Catalog.Infrastructure;

namespace SPSApi.Modules.Catalog.Features.DeleteAssetModel;

public static class DeleteAssetModelEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapDelete("/api/catalog/asset-models/{id:int}", async (
      int id,
      [FromServices] CatalogDbContext db,
      CancellationToken ct
    ) =>
    {
      var model = await db.AssetModels.FirstOrDefaultAsync(m => m.Id == id, ct);
      if (model is null)
        return Results.NotFound(new { error = $"No existe un modelo con Id {id}." });

      db.AssetModels.Remove(model);
      await db.SaveChangesAsync(ct);
      return Results.NoContent();
    }
    ).WithTags("Catalog").WithName("DeleteAssetModel");
  }
}
