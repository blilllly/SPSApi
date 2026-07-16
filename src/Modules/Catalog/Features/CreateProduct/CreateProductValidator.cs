using FluentValidation;

namespace SPSApi.Modules.Catalog.Features.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
  public CreateProductValidator()
  {
    RuleFor(x => x.Sku).NotEmpty().MaximumLength(60);
    RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    RuleFor(x => x.UnitOfMeasure).NotEmpty().MaximumLength(10);
    RuleFor(x => x.Barcode).MaximumLength(60);

    RuleFor(x => x.EstimatedCost).GreaterThan(0).When(x => x.EstimatedCost.HasValue).WithMessage("El costo estimado debe ser mayor que cero.");
    RuleFor(x => x.EstimatedPageYield).GreaterThan(0).When(x => x.EstimatedPageYield.HasValue).WithMessage("la duración estimada en páginas debe ser mayor que cero.");
  }
}
