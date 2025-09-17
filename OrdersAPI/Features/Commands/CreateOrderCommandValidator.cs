using FluentValidation;

namespace OrdersAPI.Features.Commands;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.");

        RuleFor(x => x.TotalCost)
            .GreaterThan(0).WithMessage("Total cost must be greater than zero.");
    }
}
