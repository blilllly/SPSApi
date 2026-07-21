using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace SPSApi.Modules.Catalog.Features.DeleteProduct;

public static class DeleteProductEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapDelete("/api/catalog/products/{id:int}", async (
      int id,
      [FromServices] DeleteProductHandler handler,
      CancellationToken ct
    ) =>
    {
      var result = await handler.HandleAsync(id, ct);
      return result.IsSuccess
        ? Results.NoContent()
        : Results.NotFound(new { error = result.Error });
    }
    ).WithName("Catalog").WithName("DeleteProduct");
  }
}
