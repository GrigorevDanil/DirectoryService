using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedService.Core.Handlers;

namespace DirectoryService.Application;

public static class Registration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return
            services
                .AddHandlers(typeof(Registration).Assembly)
                .AddValidatorsFromAssembly(typeof(Registration).Assembly);
    }
}