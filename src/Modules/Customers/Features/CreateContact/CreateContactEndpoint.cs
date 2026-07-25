using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace SPSApi.Modules.Customers.Features.CreateContact;

public static class CreateContactEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPost("/api/customers/contacts", async (
      CreateContactCommand cmd,
      [FromServices] IValidator<CreateContactCommand> validator,
      [FromServices] CreateContactHandler handler,
      CancellationToken ct) =>
      {
        var validation = await validator.ValidateAsync(cmd, ct);
        if (!validation.IsValid)
          return Results.ValidationProblem(validation.ToDictionary());

        var result = await handler.HandleAsync(cmd, ct);
        return result.IsSuccess
          ? Results.Created($"/api/customers/contacts/{result.Value!.Id}", result.Value)
          : Results.Conflict(new { error = result.Error });
      }
    ).WithTags("Customers").WithName("CreateContact");
  }
}
