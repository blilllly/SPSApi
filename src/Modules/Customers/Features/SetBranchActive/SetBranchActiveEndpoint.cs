using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.SetBranchActive;

public record SetActiveBody(bool IsActive);

public static class SetBranchActiveEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPatch("/api/customers/branches/{id:int}", async (
      int id,
      SetActiveBody body,
      [FromServices] CustomersDbContext db,
      CancellationToken ct
    ) =>
    {
      var branch = await db.Branches.FirstOrDefaultAsync(b => b.Id == id, ct);
      if (branch is null)
        return Results.NotFound(new { error = $"No existe una sucursal con Id {id}." });

      branch.IsActive = body.IsActive;
      await db.SaveChangesAsync(ct);

      return Results.Ok(new { branch.Id, branch.Name, branch.IsActive });
    }
    ).WithTags("Customers").WithName("SetBranchActive");
  }
}
