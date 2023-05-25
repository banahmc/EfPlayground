using System.Net;
using Carter;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Model.Domain.Customers;

namespace Web.API.Endpoints;

public class Customers : CarterModule
{
    private const string MODULE_NAME = nameof(Customers);

    public Customers() : base($"/{MODULE_NAME}")
    {
        WithTags(MODULE_NAME);
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
            var result = await sender.Send(new GetCustomerByIdQuery(customerId), ctx.RequestAborted);
            return result.Match(
                customer => Results.Ok(customer),
                _ => Results.NotFound(WellKnownProblemDetails.NotFound(ctx)));
        }).WithName("GetCustomerById")
        .Produces<CustomerDTO>((int)HttpStatusCode.OK)
        .Produces<ProblemDetails>((int)HttpStatusCode.NotFound);

        app.MapGet("/{customerEmail}", async (string customerEmail, HttpContext ctx, ISender sender) =>
        {
            var result = await sender.Send(new GetCustomerByEmailQuery(customerEmail), ctx.RequestAborted);
            return result.Match(
                customer => Results.Ok(customer),
                _ => Results.NotFound(WellKnownProblemDetails.NotFound(ctx)));
        }).WithName("GetCustomerByEmail")
        .Produces<CustomerDTO>((int)HttpStatusCode.OK)
        .Produces<ProblemDetails>((int)HttpStatusCode.NotFound);

        app.MapPost("/", async (CreateCustomerCommand command, HttpContext ctx, ISender sender) =>
        {
            var result = await sender.Send(command, ctx.RequestAborted);
            return result.Match(
                customer => Results.CreatedAtRoute("GetCustomerById", new { customerId = customer.Id }, customer),
                validationFailed => Results.BadRequest(new ValidationProblemDetails(validationFailed.Errors)));
        }).WithName("CreateCustomer")
        .Produces<CustomerDTO>((int)HttpStatusCode.OK)
        .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest);
    }
}
