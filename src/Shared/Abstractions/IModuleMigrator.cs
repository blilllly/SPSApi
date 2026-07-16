namespace SPSApi.Shared.Abstractions;

public interface IModuleMigrator
{
  int Order { get; }

  Task MigrateAsync(CancellationToken ct = default);
}
