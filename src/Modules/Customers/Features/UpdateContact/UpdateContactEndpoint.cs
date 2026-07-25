using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.UpdateContact;

public record UpdateContactBody(
  int? CustomerId, int? BranchId, string Name, string? Email, string? Phone, string? Position);

public static class UpdateContactEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPut("/api/customers/contacts/{id:int}", async (
      int id,
      UpdateContactBody body,
      [FromServices] CustomersDbContext db,
      CancellationToken ct
    ) =>
    {
      if (string.IsNullOrWhiteSpace(body.Name))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
          ["name"] = ["El nombre del contacto es obligatorio."]
        });

      if (body.CustomerId is null && body.BranchId is null)
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
          ["customerId"] = ["El contacto debe pertenecer a un cliente (CustomerId) o a una sucursal (BranchId)."]
        });

      var contact = await db.Contacts.FirstOrDefaultAsync(c => c.Id == id, ct);
      if (contact is null)
        return Results.NotFound(new { error = $"No existe un contacto con Id {id}." });

      if (body.CustomerId is not null && !await db.Customers.AnyAsync(c => c.Id == body.CustomerId, ct))
        return Results.NotFound(new { error = $"No existe un cliente con Id {body.CustomerId}." });

      if (body.BranchId is not null && !await db.Branches.AnyAsync(b => b.Id == body.BranchId, ct))
        return Results.NotFound(new { error = $"No existe una sucursal con Id {body.BranchId}." });

      contact.CustomerId = body.CustomerId;
      contact.BranchId = body.BranchId;
      contact.Name = body.Name.Trim();
      contact.Email = body.Email?.Trim();
      contact.Phone = body.Phone?.Trim();
      contact.Position = body.Position?.Trim();
      await db.SaveChangesAsync(ct);

      return Results.Ok(new
      {
        contact.Id, contact.CustomerId, contact.BranchId, contact.Name,
        contact.Email, contact.Phone, contact.Position
      });
    }
    ).WithTags("Customers").WithName("UpdateContact");
  }
}
