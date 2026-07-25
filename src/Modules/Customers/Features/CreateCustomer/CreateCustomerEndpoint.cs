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

        var customer = new Customer { Name = cmd.Name.Trim(), TaxId = cmd.TaxId?.Trim() };
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
          query = query.Where(c => c.Name.Contains(search));

        return Results.Ok(await query
          .OrderBy(c => c.Name)
          .Select(c => new { c.Id, c.Name, c.TaxId, c.IsActive })
          .ToListAsync(ct));
      }
    ).WithTags("Customers").WithName("ListCustomers");
  }
}
