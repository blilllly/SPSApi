using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.DeleteBranch;

public static class DeleteBranchEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapDelete("/api/customers/branches/{id:int}", async (
      int id,
      [FromServices] CustomersDbContext db,
      CancellationToken ct
    ) =>
    {
      var branch = await db.Branches.FirstOrDefaultAsync(b => b.Id == id, ct);
      if (branch is null)
        return Results.NotFound(new { error = $"No existe una sucursal con Id {id}." });

      var hasContacts = await db.Contacts.AnyAsync(c => c.BranchId == id, ct);
      if (hasContacts)
        return Results.Conflict(new
        {
          error = "No se puede eliminar la sucursal: tiene contactos registrados. " +
            "Elimina los contactos primero o desactiva la sucursal."
        });

      // TODO(Assets/Tickets): cuando existan esos módulos, verificar que la sucursal
      // no tenga activos instalados (Asset.BranchId) ni tickets asociados antes de
      // permitir el borrado físico. Si existe historial, NO borrar — devolver un
      // error que sugiera desactivar (IsActive = false) para no afectar la
      // reportería. Las áreas de la sucursal se eliminan en cascada (FK Cascade).
      // Por ahora, sin activos ni tickets, el borrado es seguro.

      db.Branches.Remove(branch);
      await db.SaveChangesAsync(ct);
      return Results.NoContent();
    }
    ).WithTags("Customers").WithName("DeleteBranch");
  }
}
