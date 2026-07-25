using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.UpdateBranch;

public record UpdateBranchBody(string Name, string? Address, decimal? Latitude, decimal? Longitude);

public static class UpdateBranchEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPut("/api/customers/branches/{id:int}", async (
      int id,
      UpdateBranchBody body,
      [FromServices] CustomersDbContext db,
      CancellationToken ct
    ) =>
    {
      if (string.IsNullOrWhiteSpace(body.Name))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
          ["name"] = ["El nombre de la sucursal es obligatorio."]
        });

      var branch = await db.Branches.FirstOrDefaultAsync(b => b.Id == id, ct);
      if (branch is null)
        return Results.NotFound(new { error = $"No existe una sucursal con Id {id}." });

      if (await db.Branches.AnyAsync(
        b => b.Id != id && b.CustomerId == branch.CustomerId && b.Name == body.Name, ct))
        return Results.Conflict(new { error = $"La sucursal '{body.Name}' ya existe para este cliente." });

      branch.Name = body.Name.Trim();
      branch.Address = body.Address?.Trim();
      branch.Latitude = body.Latitude;
      branch.Longitude = body.Longitude;
      await db.SaveChangesAsync(ct);

      return Results.Ok(new
      {
        branch.Id, branch.CustomerId, branch.Name, branch.Address,
        branch.Latitude, branch.Longitude, branch.IsActive
      });
    }
    ).WithTags("Customers").WithName("UpdateBranch");
  }
}
