namespace SPSApi.Modules.Customers.Features.CreateContact;

public record CreateContactCommand(
  int CustomerId,
  int? BranchId,
  string Name,
  string? Email,
  string? Phone,
  string? Position
);
