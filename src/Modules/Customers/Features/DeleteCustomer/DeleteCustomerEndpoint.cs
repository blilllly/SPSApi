using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.DeleteCustomer;

public static class DeleteCustomerEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapDelete("/api/customers/{id:int}", async (
      int id,
      [FromServices] CustomersDbContext db,
      CancellationToken ct
    ) =>
    {
      var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == id, ct);
      if (customer is null)
        return Results.NotFound(new { error = $"No existe un cliente con Id {id}." });

      var hasBranches = await db.Branches.AnyAsync(b => b.CustomerId == id, ct);
      if (hasBranches)
        return Results.Conflict(new
        {
          error = "No se puede eliminar el cliente: tiene sucursales registradas. " +
            "Elimina las sucursales primero o desactiva el cliente."
        });

      var hasContacts = await db.Contacts.AnyAsync(c => c.CustomerId == id, ct);
      if (hasContacts)
        return Results.Conflict(new
        {
          error = "No se puede eliminar el cliente: tiene contactos registrados. " +
            "Elimina los contactos primero o desactiva el cliente."
        });

      // TODO(Assets/Tickets): cuando existan esos módulos, verificar que ninguna
      // sucursal de este cliente haya tenido activos instalados o tickets asociados
      // antes de permitir el borrado físico. Si existe historial, NO borrar —
      // devolver un error que sugiera desactivar (IsActive = false) para no afectar
      // la reportería. Por ahora, sin sucursales ni contactos, el borrado es seguro.

      db.Customers.Remove(customer);
      await db.SaveChangesAsync(ct);
      return Results.NoContent();
    }
    ).WithTags("Customers").WithName("DeleteCustomer");
  }
}
