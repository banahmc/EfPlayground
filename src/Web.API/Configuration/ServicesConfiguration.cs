using Carter;
using Model;

namespace Web.API.Configuration;

public static class ServicesConfiguration
{
    public static WebApplicationBuilder ConfigureAppServices(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }

        builder.Services.AddModel(builder.Configuration);
        builder.Services.AddWeb(builder.Environment);

        return builder;
    }

    public static void AddWeb(this IServiceCollection services, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        services.AddCarter();

        services.AddHealthChecks();
    }
}
