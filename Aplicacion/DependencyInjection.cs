using Aplicacion.Core;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Aplicacion;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicacion(
    this IServiceCollection services
)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            //configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddFluentValidationAutoValidation();
        // services.AddValidatorsFromAssemblyContaining<TransportistaCreateCommand>();
        //services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddAutoMapper(typeof(MappingProfile).Assembly);


        return services;
    }
}
