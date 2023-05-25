using Mediator;
using Microsoft.EntityFrameworkCore;
using Model.Persistence;

namespace Model.Domain.Customers;

public sealed record GetCustomerByIdQuery(int Id) : IRequest<CustomerDTO?>;

public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDTO?>
{
    private readonly ApplicationDbContext _dbContext;

    public GetCustomerByIdQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<CustomerDTO?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

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
