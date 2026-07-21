using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Catalog.Domain;
using SPSApi.Modules.Catalog.Infrastructure;

namespace SPSApi.Modules.Catalog.Features.CreateAssetModel;

public record CreateAssetModelCommand(int BrandId, string Name, AssetType AssetType, string? PartNumber, int? RatedDutyCycle, bool IsColour);

public static class CreateAssetModelEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPost("/api/catalog/asset-models", async (
      CreateAssetModelCommand cmd,
      [FromServices] CatalogDbContext db,
      CancellationToken ct
    ) =>
    {
      if (string.IsNullOrEmpty(cmd.Name))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
          ["name"] = ["El nombre del modelo es obligatorio."]
        });

      if (cmd.RatedDutyCycle is <= 0)
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
          ["RatedDutyCycle"] = ["El ciclo de vida debe ser mayor a cero"]
        });

      if (!await db.Brands.AnyAsync(b => b.Id == cmd.BrandId, ct))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
          ["brandId"] = [$"No existe la marca con Id {cmd.BrandId}."]
        });

      var duplicate = await db.AssetModels
        .AnyAsync(m => m.BrandId == cmd.BrandId && m.Name == cmd.Name, ct);

      if (duplicate)
        return Results.Conflict(new { error = "Ese modelo ya existe para esta marca." });

      var isColour = cmd.AssetType == AssetType.Printer && cmd.IsColour;

      var model = new AssetModel
      {
        BrandId = cmd.BrandId,
        Name = cmd.Name.Trim(),
        AssetType = cmd.AssetType,
        RatedDutyCycle = cmd.RatedDutyCycle,
        PartNumber = string.IsNullOrWhiteSpace(cmd.PartNumber) ? null : cmd.PartNumber.Trim(),
        IsColour = isColour
      };
      db.AssetModels.Add(model);
      await db.SaveChangesAsync(ct);

      return Results.Created($"/api/catalog/asset-models/{model.Id}",
        new { model.Id, model.BrandId, model.Name, model.AssetType, model.RatedDutyCycle, model.PartNumber, model.IsColour });
    }).WithTags("Catalog").WithName("CreateAssetModel");

    app.MapGet("/api/catalog/asset-models", async (CatalogDbContext db, CancellationToken ct) =>
      Results.Ok(await db.AssetModels.AsNoTracking()
        .OrderBy(m => m.Name)
        .Select(m => new { m.Id, m.BrandId, m.Name, m.AssetType, m.RatedDutyCycle, m.PartNumber, m.IsColour })
        .ToListAsync(ct)
      )
    ).WithTags("Catalog").WithName("ListAssetModels");
  }
}
