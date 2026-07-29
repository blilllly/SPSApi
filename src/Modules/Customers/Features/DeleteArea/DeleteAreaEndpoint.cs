using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.DeleteArea;

public static class DeleteAreaEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapDelete("/api/customers/areas/{id:int}", async (
      int id,
      [FromServices] CustomersDbContext db,
      CancellationToken ct
    ) =>
    {
      var area = await db.Areas.FirstOrDefaultAsync(a => a.Id == id, ct);
      if (area is null)
        return Results.NotFound(new { error = $"No existe un área con Id {id}." });

      // TODO(Tickets): cuando exista ese módulo, verificar que el área no tenga
      // movimientos/tickets asociados antes de permitir el borrado físico. Si existe
      // historial, NO borrar — devolver un error que sugiera desactivar
      // (IsActive = false) para no afectar la reportería. Por ahora, sin tickets,
      // el borrado es seguro.

      db.Areas.Remove(area);
      await db.SaveChangesAsync(ct);
      return Results.NoContent();
    }
    ).WithTags("Customers").WithName("DeleteArea");
  }
}
