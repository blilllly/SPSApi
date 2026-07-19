using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Features.ListProducts;

public static class ListProductsEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapGet("/api/catalog/products", async (
      [FromServices] ListProductsHandler handler,
      CancellationToken ct,
      [FromQuery] ProductCategory? category,
      [FromQuery] string? search,
      [FromQuery] bool includeInactive = false
    ) =>
    {
      var result = await handler.HandleAsync(
        new ListProductsQuery(category, search, includeInactive), ct
      );

      return Results.Ok(result);
    }).WithTags("Catalog").WithName("ListProducts");
  }
}
