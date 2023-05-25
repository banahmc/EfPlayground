
using Microsoft.AspNetCore.Mvc;

namespace Web.API;

public static class WellKnownProblemDetails
{
    public static ProblemDetails NotFound(HttpContext ctx)
    {
        return new ProblemDetails
        {
            Type = "https://example.com/errors/not-found",
            Title = "Resource Not Found",
            Detail = "The resource was not found.",
            Status = 404,
            Instance = ctx.Request.Path
        };
    }
}
