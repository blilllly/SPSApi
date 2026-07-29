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

    RuleFor(x => x.CustomerId)
      .GreaterThan(0)
      .WithMessage("El cliente (customerId) es obligatorio.");
  }
}
