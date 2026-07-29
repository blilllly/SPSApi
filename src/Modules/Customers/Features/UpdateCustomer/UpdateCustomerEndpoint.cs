using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.UpdateCustomer;

public record UpdateCustomerBody(string Name, string? TaxId);

public static class UpdateCustomerEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPut("/api/customers/{id:int}", async (
      int id,
      UpdateCustomerBody body,
      [FromServices] CustomersDbContext db,
      CancellationToken ct
    ) =>
    {
      if (string.IsNullOrWhiteSpace(body.Name))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
          ["name"] = ["El nombre del cliente es obligatorio."]
        });

      var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == id, ct);
      if (customer is null)
        return Results.NotFound(new { error = $"No existe un cliente con Id {id}." });

      if (await db.Customers.AnyAsync(c => c.Id != id && c.Name == body.Name, ct))
        return Results.Conflict(new { error = $"El cliente '{body.Name}' ya existe." });

      var taxId = string.IsNullOrWhiteSpace(body.TaxId) ? null : body.TaxId.Trim();

      if (taxId is not null && !taxId.All(char.IsDigit))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
          ["taxId"] = ["El RUC/TaxId debe contener solo dígitos."]
        });

      if (taxId is not null && await db.Customers.AnyAsync(c => c.Id != id && c.TaxId == taxId, ct))
        return Results.Conflict(new { error = $"Ya existe un cliente con el RUC/TaxId '{taxId}'." });

      customer.Name = body.Name.Trim();
      customer.TaxId = taxId;
      await db.SaveChangesAsync(ct);

      return Results.Ok(new { customer.Id, customer.Name, customer.TaxId, customer.IsActive });
    }
    ).WithTags("Customers").WithName("UpdateCustomer");
  }
}
