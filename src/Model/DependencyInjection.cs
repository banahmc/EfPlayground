using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model.Persistence;
using Model.Services.SystemClock;

namespace Model;

public static class DependencyInjection
{
    public static void AddModel(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<ISystemClock, DefaultSystemClock>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });

        services.AddMediator(options =>
        {
            options.Namespace = "Mediator.SG";
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        services.AddValidatorsFromAssemblyContaining<ApplicationDbContext>();
    }
}
