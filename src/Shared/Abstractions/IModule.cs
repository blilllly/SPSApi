using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SPSApi.Shared.Abstractions;

public interface IModule
{
  void RegisterServices(IServiceCollection services, IConfiguration config);
  void MapEndpoints(IEndpointRouteBuilder endpoints);
}
