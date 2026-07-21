using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SPSApi.Modules.Catalog.Features.CreateAssetModel;
using SPSApi.Modules.Catalog.Features.CreateBrand;
using SPSApi.Modules.Catalog.Features.CreateProduct;
using SPSApi.Modules.Catalog.Features.DeleteAssetModel;
using SPSApi.Modules.Catalog.Features.DeleteBrand;
using SPSApi.Modules.Catalog.Features.DeleteProduct;
using SPSApi.Modules.Catalog.Features.ListPartNumbers;
using SPSApi.Modules.Catalog.Features.ListProducts;
using SPSApi.Modules.Catalog.Features.SetProductActive;
using SPSApi.Modules.Catalog.Features.UpdateProduct;
using SPSApi.Modules.Catalog.Infrastructure;
using SPSApi.Shared.Abstractions;

namespace SPSApi.Modules.Catalog;

public class CatalogModule : IModule
{
  public void MapEndpoints(IEndpointRouteBuilder endpoints)
  {
    // Products
    CreateProductEndpoint.Map(endpoints);
    ListProductsEndpoint.Map(endpoints);
    UpdateProductEndpoint.Map(endpoints);
    DeleteProductEndpoint.Map(endpoints);
    SetProductActiveEndpoint.Map(endpoints);
    ListPartNumbersEndpoint.Map(endpoints);

    // Brands
    CreateBrandEndpoint.Map(endpoints);
    DeleteBrandEndpoint.Map(endpoints);

    // Asset models
    CreateAssetModelEndpoint.Map(endpoints);
    DeleteAssetModelEndpoint.Map(endpoints);
  }

  public void RegisterServices(IServiceCollection services, IConfiguration config)
  {
    services.AddDbContext<CatalogDbContext>(opt =>
      opt.UseSqlServer(
        config.GetConnectionString("Default"),
        sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "catalog")
      )
    );

    services.AddScoped<IModuleMigrator, CatalogModuleMigrator>();

    // Handlers
    services.AddScoped<CreateProductHandler>();
    services.AddScoped<ListProductsHandler>();
    services.AddScoped<UpdateProductHandler>();
    services.AddScoped<DeleteProductHandler>();

    // Validators
    services.AddScoped<IValidator<CreateProductCommand>, CreateProductValidator>();
  }
}
