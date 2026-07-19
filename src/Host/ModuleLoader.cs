using System.Reflection;
using System.Runtime.Loader;
using SPSApi.Shared.Abstractions;

namespace SPSApi.Host;

public static class ModuleLoader
{
  private static IReadOnlyList<IModule>? _cache;

  public static IReadOnlyList<IModule> DiscoverModules()
  {
    if (_cache is not null) return _cache;

    var dir = AppContext.BaseDirectory;

    foreach (var dll in Directory.GetFiles(dir, "SPSApi.Modules.*.dll"))
    {
      var name = Path.GetFileNameWithoutExtension(dll);
      var alreadyLoaded = AssemblyLoadContext.Default.Assemblies
        .Any(a => a.GetName().Name == name);

      if (!alreadyLoaded)
        AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
    }

    _cache = AssemblyLoadContext.Default.Assemblies
      .Where(a => a.GetName().Name?.StartsWith("SPSApi.Modules.") == true)
      .SelectMany(SafeGetTypes)
      .Where(t => typeof(IModule).IsAssignableFrom(t)
        && t is { IsInterface: false, IsAbstract: false })
      .Select(t => (IModule)Activator.CreateInstance(t)!)
      .ToList();

    return _cache;
  }

  private static IEnumerable<Type> SafeGetTypes(Assembly a)
  {
    try
    {
      return a.GetTypes();
    }
    catch (ReflectionTypeLoadException ex)
    {
      return ex.Types.Where(t => t is not null)!;
    }
  }
}
