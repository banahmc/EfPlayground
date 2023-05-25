using FluentValidation;

namespace Model.Domain.Customers;

public class CreateCustomerValidator : AbstractValidator<CustomerEntity>
{
    public CreateCustomerValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.Email)
            .NotEmpty()
            .MaximumLength(255)
            .EmailAddress();

        RuleFor(c => c.CreatedOn)
            .GreaterThan(DateTimeOffset.MinValue);
    }
}
