using Carter;

namespace Web.API.Configuration;

public static class RequestPipelineConfiguration
{
    public static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapCarter();

        app.UseHealthChecks("/health-check");

        return app;
    }
}
