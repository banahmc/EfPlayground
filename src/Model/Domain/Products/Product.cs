namespace Model.Domain.Products;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }

    public DateTimeOffset CreatedOn { get; init; }
    public DateTimeOffset? ModifiedOn { get; init; }
}
