namespace Model.Domain.Customers;

public class CustomerEntity
{
    public int Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;

    public DateTimeOffset CreatedOn { get; init; }
    public DateTimeOffset? ModifiedOn { get; init; }
}
