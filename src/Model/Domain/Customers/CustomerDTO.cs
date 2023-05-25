namespace Model.Domain.Customers;

public record CustomerDTO(
    int Id,
    string FirstName,
    string LastName,
    string Email);
