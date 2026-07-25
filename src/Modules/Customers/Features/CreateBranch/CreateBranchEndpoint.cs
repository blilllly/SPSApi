using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Domain;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.CreateBranch;

public record CreateBranchCommand(
  int CustomerId, string Name, string? Address, decimal? Latitude, decimal? Longitude);

public static class CreateBranchEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
  {
    app.MapPost("/api/customers/branches", async (
      CreateBranchCommand cmd, [FromServices] CustomersDbContext db, CancellationToken ct) =>
      {
        if (string.IsNullOrWhiteSpace(cmd.Name))
          return Results.ValidationProblem(new Dictionary<string, string[]>
          {
            ["name"] = ["El nombre de la sucursal es obligatorio."]
          });

        if (!await db.Customers.AnyAsync(c => c.Id == cmd.CustomerId, ct))
          return Results.NotFound(new { error = $"No existe un cliente con Id {cmd.CustomerId}." });

        if (await db.Branches.AnyAsync(b => b.CustomerId == cmd.CustomerId && b.Name == cmd.Name, ct))
          return Results.Conflict(new { error = $"La sucursal '{cmd.Name}' ya existe para este cliente." });

        var branch = new Branch
        {
          CustomerId = cmd.CustomerId,
          Name = cmd.Name.Trim(),
          Address = cmd.Address?.Trim(),
          Latitude = cmd.Latitude,
          Longitude = cmd.Longitude
        };
        db.Branches.Add(branch);
        await db.SaveChangesAsync(ct);

        return Results.Created($"/api/customers/branches/{branch.Id}", new
        {
          branch.Id, branch.CustomerId, branch.Name, branch.Address,
          branch.Latitude, branch.Longitude, branch.IsActive
        });
      }
    ).WithTags("Customers").WithName("CreateBranch");

    app.MapGet("/api/customers/branches", async (
      [FromServices] CustomersDbContext db,
      CancellationToken ct,
      [FromQuery] int? customerId,
      [FromQuery] bool includeInactive = false) =>
      {
        var query = db.Branches.AsNoTracking();

        if (customerId is not null)
          query = query.Where(b => b.CustomerId == customerId);

        if (!includeInactive)
          query = query.Where(b => b.IsActive);

        return Results.Ok(await query
          .OrderBy(b => b.Name)
          .Select(b => new
          {
            b.Id, b.CustomerId, b.Name, b.Address, b.Latitude, b.Longitude, b.IsActive
          })
          .ToListAsync(ct));
      }
    ).WithTags("Customers").WithName("ListBranches");
  }
}
