using Mediator;
using Model.Persistence;
using Model.Services.SystemClock;

namespace Model.Domain.Customers;

public sealed record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string Email)
    : ICommand<CustomerDTO>;

public sealed class CrateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CustomerDTO>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISystemClock _systemClock;

    public CrateCustomerCommandHandler(ApplicationDbContext dbContext, ISystemClock systemClock)
    {
        _dbContext = dbContext;
        _systemClock = systemClock;
    }

    public async ValueTask<CustomerDTO> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        CustomerEntity newCustomer = new CustomerEntity
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            CreatedOn = _systemClock.UtcNow
        };

        _dbContext.Customers.Add(newCustomer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        CustomerEntity? customer = await _dbContext.Customers
            .FindAsync(keyValues: new object[] { newCustomer.Id }, cancellationToken);

        return new CustomerDTO(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email
        );
    }
}
