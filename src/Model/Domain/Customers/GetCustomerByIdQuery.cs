using Mediator;
using Microsoft.EntityFrameworkCore;
using Model.Common.Results;
using Model.Persistence;
using OneOf;

namespace Model.Domain.Customers;

public sealed record GetCustomerByIdQuery(int Id) : IRequest<OneOf<CustomerDTO, NotFound>>;

public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, OneOf<CustomerDTO, NotFound>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetCustomerByIdQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<OneOf<CustomerDTO, NotFound>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

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
