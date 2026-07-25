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

        if (customerId is not null)
          query = query.Where(c => c.CustomerId == customerId);

        if (branchId is not null)
          query = query.Where(c => c.BranchId == branchId);

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
