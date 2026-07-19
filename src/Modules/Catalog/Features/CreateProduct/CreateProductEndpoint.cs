using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace SPSApi.Modules.Catalog.Features.CreateProduct;

public class CreateProductEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPost("/api/catalog/products", async (
      CreateProductCommand cmd,
      [FromServices] IValidator<CreateProductCommand> validator,
      [FromServices] CreateProductHandler handler,
      CancellationToken ct) =>
      {
        var validation = await validator.ValidateAsync(cmd, ct);
        if (!validation.IsValid)
          return Results.ValidationProblem(validation.ToDictionary());

        var result = await handler.HandleAsync(cmd, ct);
        return result.IsSuccess
          ? Results.Created($"/api/catalog/products/{result.Value!.Id}", result.Value)
          : Results.Conflict(new { error = result.Error });
      }
    ).WithTags("Catalog").WithName("CreateProduct");
  }
}
