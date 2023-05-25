using Mediator;
using Microsoft.EntityFrameworkCore;
using Model.Common.Results;
using Model.Persistence;
using OneOf;

namespace Model.Domain.Customers;

public sealed record GetCustomerByEmailQuery(string Email) : IRequest<OneOf<CustomerDTO, NotFound>>;

public sealed class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, OneOf<CustomerDTO, NotFound>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetCustomerByEmailQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<OneOf<CustomerDTO, NotFound>> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == request.Email.ToLower().Trim(), cancellationToken);

        if (customer is null)
        {
            return new NotFound();
        }

        return new CustomerDTO(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email
        );
    }
}
