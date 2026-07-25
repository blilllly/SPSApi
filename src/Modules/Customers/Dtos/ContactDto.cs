namespace SPSApi.Modules.Customers.Dtos;

public record ContactDto(
  int Id,
  int? CustomerId,
  int? BranchId,
  string Name,
  string? Email,
  string? Phone,
  string? Position
);
