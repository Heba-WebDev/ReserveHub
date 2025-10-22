namespace ReserveHub.Application.Services.Contracts;

public interface IEmailService
{
    Task SendEmailConfirmationAsync(string email, string confirmationLink);
    Task SendPasswordResetAsync(string email, string resetLink);
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
}