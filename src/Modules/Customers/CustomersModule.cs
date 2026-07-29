using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SPSApi.Modules.Customers.Features.CreateArea;
using SPSApi.Modules.Customers.Features.CreateBranch;
using SPSApi.Modules.Customers.Features.CreateContact;
using SPSApi.Modules.Customers.Features.CreateCustomer;
using SPSApi.Modules.Customers.Features.DeleteArea;
using SPSApi.Modules.Customers.Features.DeleteBranch;
using SPSApi.Modules.Customers.Features.DeleteContact;
using SPSApi.Modules.Customers.Features.DeleteCustomer;
using SPSApi.Modules.Customers.Features.ListContacts;
using SPSApi.Modules.Customers.Features.SetAreaActive;
using SPSApi.Modules.Customers.Features.SetBranchActive;
using SPSApi.Modules.Customers.Features.SetCustomerActive;
using SPSApi.Modules.Customers.Features.UpdateArea;
using SPSApi.Modules.Customers.Features.UpdateBranch;
using SPSApi.Modules.Customers.Features.UpdateContact;
using SPSApi.Modules.Customers.Features.UpdateCustomer;
using SPSApi.Modules.Customers.Infrastructure;
using SPSApi.Shared.Abstractions;

namespace SPSApi.Modules.Customers;

public class CustomersModule : IModule
{
  public void MapEndpoints(IEndpointRouteBuilder endpoints)
  {
    // Customers
    CreateCustomerEndpoint.Map(endpoints);
    UpdateCustomerEndpoint.Map(endpoints);
    DeleteCustomerEndpoint.Map(endpoints);
    SetCustomerActiveEndpoint.Map(endpoints);

    // Branches
    CreateBranchEndpoint.Map(endpoints);
    UpdateBranchEndpoint.Map(endpoints);
    DeleteBranchEndpoint.Map(endpoints);
    SetBranchActiveEndpoint.Map(endpoints);

    // Areas
    CreateAreaEndpoint.Map(endpoints);
    UpdateAreaEndpoint.Map(endpoints);
    DeleteAreaEndpoint.Map(endpoints);
    SetAreaActiveEndpoint.Map(endpoints);

    // Contacts
    CreateContactEndpoint.Map(endpoints);
    ListContactsEndpoint.Map(endpoints);
    UpdateContactEndpoint.Map(endpoints);
    DeleteContactEndpoint.Map(endpoints);
  }

  public void RegisterServices(IServiceCollection services, IConfiguration config)
  {
    services.AddDbContext<CustomersDbContext>(opt =>
      opt.UseSqlServer(
        config.GetConnectionString("Default"),
        sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "customers")
      )
    );

    services.AddScoped<IModuleMigrator, CustomersModuleMigrator>();

    // Handlers
    services.AddScoped<CreateContactHandler>();

    // Validators
    services.AddScoped<IValidator<CreateContactCommand>, CreateContactValidator>();
  }
}
