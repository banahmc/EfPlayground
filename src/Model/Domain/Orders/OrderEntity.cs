namespace Model.Domain.Orders;

public class OrderEntity
{
    public int Id { get; set; }

    public DateTimeOffset CreatedOn { get; init; }
    public DateTimeOffset? ModifiedOn { get; init; }
}
