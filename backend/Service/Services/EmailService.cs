using Application;
using Domain.Config;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailApp
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailOptions)
    {
        _emailSettings = emailOptions.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(to))
            throw new ArgumentException("Destinatário é obrigatório.");

        ValidateSettings();

        using var message = new MailMessage();
        message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
        message.To.Add(new MailAddress(to));
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = false;

        using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword)
        };

        await smtpClient.SendMailAsync(message);
    }

    private void ValidateSettings()
    {
        if (string.IsNullOrWhiteSpace(_emailSettings.SmtpServer))
            throw new InvalidOperationException("EmailSettings:SmtpServer não foi configurado.");

        if (_emailSettings.Port <= 0)
            throw new InvalidOperationException("EmailSettings:Port não foi configurado corretamente.");

        if (string.IsNullOrWhiteSpace(_emailSettings.SenderEmail))
            throw new InvalidOperationException("EmailSettings:SenderEmail não foi configurado.");

        if (string.IsNullOrWhiteSpace(_emailSettings.SenderPassword))
            throw new InvalidOperationException("EmailSettings:SenderPassword não foi configurado.");
    }
}