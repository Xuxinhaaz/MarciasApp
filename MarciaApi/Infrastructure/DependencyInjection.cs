using MarciaApi.Application.Services.Email;
using MarciaApi.Infrastructure.Services.Auth.Authentication;
using MarciaApi.Infrastructure.Services.Auth.Authorizarion;
using MarciaApi.Infrastructure.Services.Email;

namespace MarciaApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        
        return services;
    }
}