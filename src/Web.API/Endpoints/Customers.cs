using Carter;
using Mediator;
using Model.Domain.Customers;

namespace Web.API.Endpoints;

public class Customers : CarterModule
{
    public Customers() : base("/customers")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ISender sender) =>
        {
            var data = await sender.Send(new GetAllCustomersQuery());
            return Results.Ok(data);
        }).WithName("GetAllCustomers");

        app.MapGet("/{customerId:int}", async (int customerId, ISender sender) =>
        {
            var data = await sender.Send(new GetCustomerByIdQuery(customerId));
            return Results.Ok(data);
        }).WithName("GetCustomerById");

        app.MapGet("/{customerEmail}", async (string customerEmail, ISender sender) =>
        {
            var data = await sender.Send(new GetCustomerByEmailQuery(customerEmail));
            return Results.Ok(data);
        }).WithName("GetCustomerByEmail");

        app.MapPost("/", async (CreateCustomerCommand command, HttpContext ctx, ISender sender) =>
        {
            var customer = await sender.Send(command);
            return Results.CreatedAtRoute("GetCustomerById", new { customerId = customer.Id }, customer);
        }).WithName("CreateCustomer");
    }
}
