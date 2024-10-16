using MimeKit;
using SmtpClient = System.Net.Mail.SmtpClient;
using System;
using MailKit.Security;
using MarciaApi.Application.Services.Email;

namespace MarciaApi.Infrastructure.Services.Email;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task SendEmailAsync(string email, string subject, string body)
    {

        try
        {
            MimeMessage message = new();
            
            message.From.Add(new MailboxAddress(_configuration["MailKitSettings:MyEmailName"], _configuration["MailKitSettings:MyEmail"]));
            
            message.To.Add(new MailboxAddress("Client", email));
            message.Subject = subject;
            message.Body = new TextPart()
            {
                Text = body
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_configuration["MailKitSettings:EmailServer"], 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuration["MailKitSettings:MyEmail"], _configuration["MailKitSettings:MyEmailPassword"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}