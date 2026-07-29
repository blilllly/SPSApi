using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.UpdateContact;

public record UpdateContactBody(
  int CustomerId, int? BranchId, string Name, string? Email, string? Phone, string? Position);

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

      if (body.CustomerId <= 0)
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
          ["customerId"] = ["El cliente (customerId) es obligatorio."]
        });

      var contact = await db.Contacts.FirstOrDefaultAsync(c => c.Id == id, ct);
      if (contact is null)
        return Results.NotFound(new { error = $"No existe un contacto con Id {id}." });

      if (!await db.Customers.AnyAsync(c => c.Id == body.CustomerId, ct))
        return Results.NotFound(new { error = $"No existe un cliente con Id {body.CustomerId}." });

      if (body.BranchId is not null)
      {
        var branchCustomerId = await db.Branches.AsNoTracking()
          .Where(b => b.Id == body.BranchId)
          .Select(b => (int?)b.CustomerId)
          .FirstOrDefaultAsync(ct);

        if (branchCustomerId is null)
          return Results.NotFound(new { error = $"No existe una sucursal con Id {body.BranchId}." });

        if (branchCustomerId != body.CustomerId)
          return Results.Conflict(new { error = $"La sucursal {body.BranchId} no pertenece al cliente {body.CustomerId}." });
      }

      var name = body.Name.Trim();

      if (body.BranchId is not null)
      {
        if (await db.Contacts.AnyAsync(c => c.Id != id && c.BranchId == body.BranchId && c.Name == name, ct))
          return Results.Conflict(new { error = $"Ya existe un contacto llamado '{name}' para esta sucursal." });
      }
      else if (await db.Contacts.AnyAsync(c => c.Id != id && c.CustomerId == body.CustomerId && c.BranchId == null && c.Name == name, ct))
        return Results.Conflict(new { error = $"Ya existe un contacto llamado '{name}' para este cliente." });

      contact.CustomerId = body.CustomerId;
      contact.BranchId = body.BranchId;
      contact.Name = name;
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
