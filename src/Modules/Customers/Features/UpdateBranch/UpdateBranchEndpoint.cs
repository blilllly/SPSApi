using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Domain;
using SPSApi.Modules.Customers.Infrastructure;

namespace SPSApi.Modules.Customers.Features.UpdateBranch;

public record UpdateBranchBody(
  string Name,
  string MainStreet, string? SecondaryStreet, string? BuildingNumber,
  string? City, string Province, string? PostalCode, string? Reference,
  decimal? Latitude, decimal? Longitude);

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

      if (string.IsNullOrWhiteSpace(body.MainStreet))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
          ["mainStreet"] = ["La calle principal es obligatoria."]
        });

      if (string.IsNullOrWhiteSpace(body.Province))
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
          ["province"] = ["La provincia es obligatoria."]
        });

      var branch = await db.Branches.FirstOrDefaultAsync(b => b.Id == id, ct);
      if (branch is null)
        return Results.NotFound(new { error = $"No existe una sucursal con Id {id}." });

      if (await db.Branches.AnyAsync(
        b => b.Id != id && b.CustomerId == branch.CustomerId && b.Name == body.Name, ct))
        return Results.Conflict(new { error = $"La sucursal '{body.Name}' ya existe para este cliente." });

      var name = body.Name.Trim();
      var mainStreet = body.MainStreet.Trim();
      var secondaryStreet = string.IsNullOrWhiteSpace(body.SecondaryStreet) ? null : body.SecondaryStreet.Trim();
      var buildingNumber = string.IsNullOrWhiteSpace(body.BuildingNumber) ? null : body.BuildingNumber.Trim();
      var city = string.IsNullOrWhiteSpace(body.City) ? null : body.City.Trim();
      var province = body.Province.Trim();
      var postalCode = string.IsNullOrWhiteSpace(body.PostalCode) ? null : body.PostalCode.Trim();
      var reference = string.IsNullOrWhiteSpace(body.Reference) ? null : body.Reference.Trim();

      if (branch.Name == name &&
        branch.Address.MainStreet == mainStreet &&
        branch.Address.SecondaryStreet == secondaryStreet &&
        branch.Address.BuildingNumber == buildingNumber &&
        branch.Address.City == city &&
        branch.Address.Province == province &&
        branch.Address.PostalCode == postalCode &&
        branch.Address.Reference == reference &&
        branch.Address.Latitude == body.Latitude &&
        branch.Address.Longitude == body.Longitude)
        return Results.Conflict(new { error = "No hay cambios respecto a los datos actuales de la sucursal." });

      branch.Name = name;
      branch.Address = new Address
      {
        MainStreet = mainStreet,
        SecondaryStreet = secondaryStreet,
        BuildingNumber = buildingNumber,
        City = city,
        Province = province,
        PostalCode = postalCode,
        Reference = reference,
        Latitude = body.Latitude,
        Longitude = body.Longitude
      };
      await db.SaveChangesAsync(ct);

      return Results.Ok(new
      {
        branch.Id, branch.CustomerId, branch.Name,
        branch.Address.MainStreet, branch.Address.SecondaryStreet, branch.Address.BuildingNumber,
        branch.Address.City, branch.Address.Province, branch.Address.PostalCode, branch.Address.Reference,
        branch.Address.Latitude, branch.Address.Longitude,
        branch.IsActive
      });
    }
    ).WithTags("Customers").WithName("UpdateBranch");
  }
}
