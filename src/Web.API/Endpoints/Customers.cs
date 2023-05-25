using System.Net;
using Carter;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Model.Domain.Customers;

namespace Web.API.Endpoints;

public class Customers : CarterModule
{
    public Customers() : base("/customers")
    {
        WithTags(nameof(Customers));
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (HttpContext ctx, ISender sender) =>
        {
            var data = await sender.Send(new GetAllCustomersQuery(), ctx.RequestAborted);
            return Results.Ok(data);
        }).WithName("GetAllCustomers")
        .Produces<IEnumerable<CustomerDTO>>((int)HttpStatusCode.OK);

        app.MapGet("/{customerId:int}", async (int customerId, HttpContext ctx, ISender sender) =>
        {
            var data = await sender.Send(new GetCustomerByIdQuery(customerId), ctx.RequestAborted);
            return Results.Ok(data);
        }).WithName("GetCustomerById")
        .Produces<CustomerDTO>((int)HttpStatusCode.OK)
        .Produces((int)HttpStatusCode.NotFound);

        app.MapGet("/{customerEmail}", async (string customerEmail, HttpContext ctx, ISender sender) =>
        {
            var data = await sender.Send(new GetCustomerByEmailQuery(customerEmail), ctx.RequestAborted);
            return Results.Ok(data);
        }).WithName("GetCustomerByEmail")
        .Produces<CustomerDTO>((int)HttpStatusCode.OK)
        .Produces((int)HttpStatusCode.NotFound);

        app.MapPost("/", async (CreateCustomerCommand command, HttpContext ctx, ISender sender) =>
        {
            var customer = await sender.Send(command, ctx.RequestAborted);
            return Results.CreatedAtRoute("GetCustomerById", new { customerId = customer.Id }, customer);
        }).WithName("CreateCustomer")
        .Produces<CustomerDTO>((int)HttpStatusCode.OK)
        .Produces<ProblemDetails>((int)HttpStatusCode.BadRequest);
    }
}
