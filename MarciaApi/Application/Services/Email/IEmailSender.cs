namespace MarciaApi.Application.Services.Email;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string body);
}