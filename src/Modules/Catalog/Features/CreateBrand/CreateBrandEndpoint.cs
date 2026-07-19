using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Catalog.Domain;
using SPSApi.Modules.Catalog.Infrastructure;

namespace SPSApi.Modules.Catalog.Features.CreateBrand;

public record CreateBrandCommand(string Name);

public static class CreateBrandEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPost("api/catalog/brands", async (
      CreateBrandCommand cmd, CatalogDbContext db, CancellationToken ct) =>
      {
        if (string.IsNullOrWhiteSpace(cmd.Name))
          return Results.ValidationProblem(new Dictionary<string, string[]>
          {
            ["name"] = ["El nombre de la marca es obligatorio."]
          });

        if (await db.Brands.AnyAsync(x => x.Name == cmd.Name, ct))
          return Results.Conflict(new { error = $"La marca '{cmd.Name}' ya existe." });

        var brand = new Brand { Name = cmd.Name.Trim() };
        db.Brands.Add(brand);
        await db.SaveChangesAsync(ct);

        return Results.Created($"/api/catalog/brands/{brand.Id}", new { brand.Id, brand.Name });
      }
    ).WithTags("Catalog").WithName("CreateBrand");

    app.MapGet("/api/catalog/brands", async (CatalogDbContext db, CancellationToken ct) =>
      Results.Ok(await db.Brands.AsNoTracking()
        .OrderBy(b => b.Name)
        .Select(b => new { b.Id, b.Name })
        .ToListAsync(ct)
      )
    ).WithTags("Catalog").WithName("ListBrands");
  }

}
