using MimeKit;
using SmtpClient = System.Net.Mail.SmtpClient;
using System;
using MailKit.Security;

namespace MarciaApi.Infrastructure.Services.Email;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string body)
    {

        try
        {
            MimeMessage message = new();
            
            message.From.Add(new MailboxAddress("marmitaria da marcia", "caiomartinsdiesel@gmail.com"));
            
            message.To.Add(new MailboxAddress("destino", "joojjunu@gmail.com"));
            message.Subject = subject;
            message.Body = new TextPart()
            {
                Text = body
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("caiomartinsdiesel@gmail.com", "hxmk dzif zmzz yyiz");
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