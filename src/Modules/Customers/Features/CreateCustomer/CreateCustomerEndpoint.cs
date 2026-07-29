using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Domain;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.CreateCustomer;

public record CreateCustomerCommand(string Name, string? TaxId);

public static class CreateCustomerEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPost("/api/customers", async (
      CreateCustomerCommand cmd, [FromServices] CustomersDbContext db, CancellationToken ct) =>
      {
        if (string.IsNullOrWhiteSpace(cmd.Name))
          return Results.ValidationProblem(new Dictionary<string, string[]>
          {
            ["name"] = ["El nombre del cliente es obligatorio."]
          });

        if (await db.Customers.AnyAsync(x => x.Name == cmd.Name, ct))
          return Results.Conflict(new { error = $"El cliente '{cmd.Name}' ya existe." });

        var taxId = string.IsNullOrWhiteSpace(cmd.TaxId) ? null : cmd.TaxId.Trim();

        if (taxId is not null && !taxId.All(char.IsDigit))
          return Results.ValidationProblem(new Dictionary<string, string[]>
          {
            ["taxId"] = ["El RUC/TaxId debe contener solo dígitos."]
          });

        if (taxId is not null && await db.Customers.AnyAsync(x => x.TaxId == taxId, ct))
          return Results.Conflict(new { error = $"Ya existe un cliente con el RUC/TaxId '{taxId}'." });

        var customer = new Customer { Name = cmd.Name.Trim(), TaxId = taxId };
        db.Customers.Add(customer);
        await db.SaveChangesAsync(ct);

        return Results.Created($"/api/customers/{customer.Id}",
          new { customer.Id, customer.Name, customer.TaxId, customer.IsActive });
      }
    ).WithTags("Customers").WithName("CreateCustomer");

    app.MapGet("/api/customers", async (
      [FromServices] CustomersDbContext db,
      CancellationToken ct,
      [FromQuery] string? search,
      [FromQuery] bool includeInactive = false) =>
      {
        var query = db.Customers.AsNoTracking();

        if (!includeInactive)
          query = query.Where(c => c.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
          query = query.Where(c => c.Name.Contains(search) || (c.TaxId != null && c.TaxId.Contains(search)));

        return Results.Ok(await query
          .OrderBy(c => c.Name)
          .Select(c => new { c.Id, c.Name, c.TaxId, c.IsActive })
          .ToListAsync(ct));
      }
    ).WithTags("Customers").WithName("ListCustomers");

    app.MapGet("/api/customers/{id:int}", async (
      int id, [FromServices] CustomersDbContext db, CancellationToken ct) =>
      {
        var customer = await db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);
        if (customer is null)
          return Results.NotFound(new { error = $"No existe un cliente con Id {id}." });

        return Results.Ok(new { customer.Id, customer.Name, customer.TaxId, customer.IsActive });
      }
    ).WithTags("Customers").WithName("GetCustomerById");
  }
}
