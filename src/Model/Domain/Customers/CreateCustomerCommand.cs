using FluentValidation;
using Mediator;
using Model.Common.Results;
using Model.Persistence;
using Model.Services.SystemClock;
using OneOf;

namespace Model.Domain.Customers;

public sealed record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string Email)
    : ICommand<OneOf<CustomerDTO, ValidationFailed>>;

public sealed class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, OneOf<CustomerDTO, ValidationFailed>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISystemClock _systemClock;
    private readonly IValidator<CustomerEntity> _validator;

    public CreateCustomerCommandHandler(
        ApplicationDbContext dbContext,
        ISystemClock systemClock,
        IValidator<CustomerEntity> validator)
    {
        _dbContext = dbContext;
        _systemClock = systemClock;
        _validator = validator;
    }

    public async ValueTask<OneOf<CustomerDTO, ValidationFailed>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        CustomerEntity newCustomer = new CustomerEntity
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            CreatedOn = _systemClock.UtcNow
        };

        var validationResult = _validator.Validate(newCustomer);
        if (!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        _dbContext.Customers.Add(newCustomer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Intentionally skipping null check via null forgiving operator.
        // We should never run into a null value case.
        // If we do then well... we have other and bigger problems.
        CustomerEntity customer = (await _dbContext.Customers
            .FindAsync(keyValues: new object[] { newCustomer.Id }, cancellationToken))!;

        return new CustomerDTO(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email
        );
    }
}
