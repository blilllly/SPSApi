using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.ListContacts;

public static class ListContactsEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapGet("/api/customers/contacts", async (
      [FromServices] CustomersDbContext db,
      CancellationToken ct,
      [FromQuery] int? customerId,
      [FromQuery] int? branchId) =>
      {
        var query = db.Contacts.AsNoTracking();

        if (branchId is not null)
        {
          // Un contacto CustomerId-level (sin BranchId) sirve a todas las sucursales
          // del cliente (ej. sistemas), aunque físicamente esté alojado en la matriz.
          var branchCustomerId = await db.Branches.AsNoTracking()
            .Where(b => b.Id == branchId)
            .Select(b => (int?)b.CustomerId)
            .FirstOrDefaultAsync(ct);

          query = query.Where(c => c.BranchId == branchId ||
            (branchCustomerId != null && c.CustomerId == branchCustomerId && c.BranchId == null));
        }
        else if (customerId is not null)
          query = query.Where(c => c.CustomerId == customerId);

        return Results.Ok(await query
          .OrderBy(c => c.Name)
          .Select(c => new
          {
            c.Id, c.CustomerId, c.BranchId, c.Name, c.Email, c.Phone, c.Position
          })
          .ToListAsync(ct));
      }
    ).WithTags("Customers").WithName("ListContacts");
  }
}
