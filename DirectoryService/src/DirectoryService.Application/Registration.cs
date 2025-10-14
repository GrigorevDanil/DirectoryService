using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions;

namespace DirectoryService.Application;

public static class Registration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return
            services
                .AddCommands()
                .AddValidators();
    }

    private static IServiceCollection AddCommands(this IServiceCollection services) =>
        services.Scan(scan =>
            scan.FromAssemblies(typeof(Registration).Assembly)
                .AddClasses(classes =>
                    classes.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());


    private static IServiceCollection AddValidators(this IServiceCollection services) =>
        services.AddValidatorsFromAssembly(typeof(Registration).Assembly);

}