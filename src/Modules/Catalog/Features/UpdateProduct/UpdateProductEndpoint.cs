using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Features.UpdateProduct;

public static class UpdateProductEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPut("/api/catalog/products/{id:int}", async (
      int id,
      UpdateProductBody body,
      [FromServices] UpdateProductHandler handler,
      CancellationToken ct
    ) =>
    {
      var cmd = new UpdateProductCommand(
        id, body.Name, body.Category, body.UnitOfMeasure,
        body.Barcode, body.EstimatedCost, body.EstimatedPageYield, body.IsActive
      );

      var result = await handler.HandleAsync(cmd, ct);

      return result.IsSuccess
        ? Results.Ok(result.Value)
        : Results.NotFound(new { error = result.Error });
    }).WithTags("Catalog").WithName("UpdateProduct");
  }
}

public record UpdateProductBody(
  string Name,
  ProductCategory Category,
  string UnitOfMeasure,
  string? Barcode,
  decimal? EstimatedCost,
  int? EstimatedPageYield,
  bool IsActive
);