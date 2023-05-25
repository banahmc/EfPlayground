using Mediator;
using Microsoft.EntityFrameworkCore;
using Model.Persistence;

namespace Model.Domain.Customers;

public sealed record GetAllCustomersQuery : IRequest<IEnumerable<CustomerDTO>>;

public sealed class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDTO>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllCustomersQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<IEnumerable<CustomerDTO>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        return (await _dbContext.Customers
                .AsNoTracking()
                .ToListAsync(cancellationToken))
            .Select(c => new CustomerDTO(c.Id, c.FirstName, c.LastName, c.Email));
    }
}
