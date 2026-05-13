namespace Application;

public interface IEmailApp
{
    Task SendEmailAsync(string to, string subject, string body);
}