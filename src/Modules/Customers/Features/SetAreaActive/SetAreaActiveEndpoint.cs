using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.SetAreaActive;

public record SetActiveBody(bool IsActive);

public static class SetAreaActiveEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPatch("/api/customers/areas/{id:int}", async (
      int id,
      SetActiveBody body,
      [FromServices] CustomersDbContext db,
      CancellationToken ct
    ) =>
    {
      var area = await db.Areas.FirstOrDefaultAsync(a => a.Id == id, ct);
      if (area is null)
        return Results.NotFound(new { error = $"No existe un área con Id {id}." });

      if (area.IsActive == body.IsActive)
        return Results.Conflict(new
        {
          error = body.IsActive
            ? "El área ya está activa, no hace falta activarla de nuevo."
            : "El área ya está desactivada, no hace falta desactivarla de nuevo."
        });

      area.IsActive = body.IsActive;
      await db.SaveChangesAsync(ct);

      return Results.Ok(new { area.Id, area.Name, area.IsActive });
    }
    ).WithTags("Customers").WithName("SetAreaActive");
  }
}
