namespace MarciaApi.Infrastructure.Services.Email;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string body);
}