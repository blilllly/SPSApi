using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.DeleteContact;

public static class DeleteContactEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapDelete("/api/customers/contacts/{id:int}", async (
      int id,
      [FromServices] CustomersDbContext db,
      CancellationToken ct
    ) =>
    {
      var contact = await db.Contacts.FirstOrDefaultAsync(c => c.Id == id, ct);
      if (contact is null)
        return Results.NotFound(new { error = $"No existe un contacto con Id {id}." });

      db.Contacts.Remove(contact);
      await db.SaveChangesAsync(ct);
      return Results.NoContent();
    }
    ).WithTags("Customers").WithName("DeleteContact");
  }
}
