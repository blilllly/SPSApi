using FluentValidation;

namespace SPSApi.Modules.Customers.Features.CreateContact;

public class CreateContactValidator : AbstractValidator<CreateContactCommand>
{
  public CreateContactValidator()
  {
    RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    RuleFor(x => x.Email).MaximumLength(254).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
    RuleFor(x => x.Phone).MaximumLength(30);
    RuleFor(x => x.Position).MaximumLength(100);

    RuleFor(x => x)
      .Must(x => x.CustomerId.HasValue || x.BranchId.HasValue)
      .WithName("customerId")
      .WithMessage("El contacto debe pertenecer a un cliente (CustomerId) o a una sucursal (BranchId).");
  }
}
