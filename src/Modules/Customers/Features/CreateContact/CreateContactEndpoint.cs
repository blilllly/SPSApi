using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.CreateContact;

public static class CreateContactEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPost("/api/customers/contacts", async (
      CreateContactCommand cmd,
      [FromServices] IValidator<CreateContactCommand> validator,
      [FromServices] CreateContactHandler handler,
      [FromServices] CustomersDbContext db,
      CancellationToken ct) =>
      {
        var validation = await validator.ValidateAsync(cmd, ct);
        if (!validation.IsValid)
          return Results.ValidationProblem(validation.ToDictionary());

        if (!await db.Customers.AnyAsync(c => c.Id == cmd.CustomerId, ct))
          return Results.NotFound(new { error = $"No existe un cliente con Id {cmd.CustomerId}." });

        if (cmd.BranchId is not null)
        {
          var branchCustomerId = await db.Branches.AsNoTracking()
            .Where(b => b.Id == cmd.BranchId)
            .Select(b => (int?)b.CustomerId)
            .FirstOrDefaultAsync(ct);

          if (branchCustomerId is null)
            return Results.NotFound(new { error = $"No existe una sucursal con Id {cmd.BranchId}." });

          if (branchCustomerId != cmd.CustomerId)
            return Results.Conflict(new { error = $"La sucursal {cmd.BranchId} no pertenece al cliente {cmd.CustomerId}." });
        }

        var result = await handler.HandleAsync(cmd, ct);
        return result.IsSuccess
          ? Results.Created($"/api/customers/contacts/{result.Value!.Id}", result.Value)
          : Results.Conflict(new { error = result.Error });
      }
    ).WithTags("Customers").WithName("CreateContact");
  }
}
