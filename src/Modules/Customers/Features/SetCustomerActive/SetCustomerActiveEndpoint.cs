using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.SetCustomerActive;

public record SetActiveBody(bool IsActive);

public static class SetCustomerActiveEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPatch("/api/customers/{id:int}", async (
      int id,
      SetActiveBody body,
      [FromServices] CustomersDbContext db,
      CancellationToken ct
    ) =>
    {
      var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == id, ct);
      if (customer is null)
        return Results.NotFound(new { error = $"No existe un cliente con Id {id}." });

      customer.IsActive = body.IsActive;
      await db.SaveChangesAsync(ct);

      return Results.Ok(new { customer.Id, customer.Name, customer.IsActive });
    }
    ).WithTags("Customers").WithName("SetCustomerActive");
  }
}
