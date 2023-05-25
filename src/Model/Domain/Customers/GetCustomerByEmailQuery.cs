using Mediator;
using Microsoft.EntityFrameworkCore;
using Model.Persistence;

namespace Model.Domain.Customers;

public sealed record GetCustomerByEmailQuery(string Email) : IRequest<CustomerDTO?>;

public sealed class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, CustomerDTO?>
{
    private readonly ApplicationDbContext _dbContext;

    public GetCustomerByEmailQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<CustomerDTO?> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase), cancellationToken);

        if (customer is null)
        {
            return null;
        }

        return new CustomerDTO(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email
        );
    }
}
