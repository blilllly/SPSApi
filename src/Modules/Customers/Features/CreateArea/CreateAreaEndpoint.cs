using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Domain;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.CreateArea;

public record CreateAreaCommand(int BranchId, string Name);

public static class CreateAreaEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPost("/api/customers/areas", async (
      CreateAreaCommand cmd, [FromServices] CustomersDbContext db, CancellationToken ct) =>
      {
        if (string.IsNullOrWhiteSpace(cmd.Name))
          return Results.ValidationProblem(new Dictionary<string, string[]>
          {
            ["name"] = ["El nombre del área es obligatorio."]
          });

        if (!await db.Branches.AnyAsync(b => b.Id == cmd.BranchId, ct))
          return Results.NotFound(new { error = $"No existe una sucursal con Id {cmd.BranchId}." });

        if (await db.Areas.AnyAsync(a => a.BranchId == cmd.BranchId && a.Name == cmd.Name, ct))
          return Results.Conflict(new { error = $"El área '{cmd.Name}' ya existe para esta sucursal." });

        var area = new Area { BranchId = cmd.BranchId, Name = cmd.Name.Trim() };
        db.Areas.Add(area);
        await db.SaveChangesAsync(ct);

        return Results.Created($"/api/customers/areas/{area.Id}",
          new { area.Id, area.BranchId, area.Name });
      }
    ).WithTags("Customers").WithName("CreateArea");

    app.MapGet("/api/customers/areas", async (
      [FromServices] CustomersDbContext db,
      CancellationToken ct,
      [FromQuery] int? branchId) =>
      {
        var query = db.Areas.AsNoTracking();

        if (branchId is not null)
          query = query.Where(a => a.BranchId == branchId);

        return Results.Ok(await query
          .OrderBy(a => a.Name)
          .Select(a => new { a.Id, a.BranchId, a.Name })
          .ToListAsync(ct));
      }
    ).WithTags("Customers").WithName("ListAreas");
  }
}
