using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.UpdateArea;

public record UpdateAreaBody(string Name);

public static class UpdateAreaEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPut("/api/customers/areas/{id:int}", async (
      int id,
      UpdateAreaBody body,
      [FromServices] CustomersDbContext db,
      CancellationToken ct
    ) =>
    {
      if (string.IsNullOrWhiteSpace(body.Name))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
          ["name"] = ["El nombre del área es obligatorio."]
        });

      var area = await db.Areas.FirstOrDefaultAsync(a => a.Id == id, ct);
      if (area is null)
        return Results.NotFound(new { error = $"No existe un área con Id {id}." });

      if (await db.Areas.AnyAsync(a => a.Id != id && a.BranchId == area.BranchId && a.Name == body.Name, ct))
        return Results.Conflict(new { error = $"El área '{body.Name}' ya existe para esta sucursal." });

      area.Name = body.Name.Trim();
      await db.SaveChangesAsync(ct);

      return Results.Ok(new { area.Id, area.BranchId, area.Name });
    }
    ).WithTags("Customers").WithName("UpdateArea");
  }
}
