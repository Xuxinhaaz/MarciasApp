using MarciaApi.Infrastructure.Services.Email;

namespace MarciaApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        
        return services;
    }
}