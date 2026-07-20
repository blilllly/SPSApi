using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Catalog.Infrastructure;

namespace SPSApi.Modules.Catalog.Features.ListPartNumbers;

public static class ListPartNumbersEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapGet("/api/catalog/part-numbers", async (
      [FromServices] CatalogDbContext db,
      CancellationToken ct,
      [FromQuery] string? search,
      [FromQuery] int take = 20
    ) =>
    {
      var query = db.Products.AsNoTracking()
        .Where(p => p.PartNumber != null);

      if (!string.IsNullOrWhiteSpace(search))
      {
        var term = search.Trim();
        query = query.Where(p => p.PartNumber!.Contains(term));
      }

      var partNumbers = await query
        .Select(p => p.PartNumber!)
        .Distinct()
        .OrderBy(pn => pn)
        .Take(Math.Clamp(take, 1, 50))
        .ToListAsync(ct);

      return Results.Ok(partNumbers);
    }
    ).WithTags("Catalog").WithName("ListPartNumbers");
  }
}
