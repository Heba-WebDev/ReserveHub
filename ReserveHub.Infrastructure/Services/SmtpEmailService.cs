using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using ReserveHub.Application.Services.Contracts;
using ReserveHub.Infrastructure.Configurations;

namespace ReserveHub.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfig;

    public SmtpEmailService(IOptions<EmailConfiguration> emailConfig)
    {
        _emailConfig = emailConfig.Value;
    }

    public async Task SendEmailConfirmationAsync(string email, string confirmationLink)
    {
        var subject = "Confirm Your Email - ReserveHub";
        var body = CreateEmailConfirmationBody(confirmationLink);
        
        await SendEmailAsync(email, subject, body);
    }

    public async Task SendPasswordResetAsync(string email, string resetLink)
    {
        var subject = "Reset Your Password - ReserveHub";
        var body = CreatePasswordResetBody(resetLink);
        
        await SendEmailAsync(email, subject, body);
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        try
        {
            using var client = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.SmtpPort);
            client.EnableSsl = _emailConfig.EnableSsl;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Timeout = 15000; // 15s
            client.Credentials = new NetworkCredential(_emailConfig.SmtpUsername, _emailConfig.SmtpPassword);
            
            using var message = new MailMessage();
            message.From = new MailAddress(_emailConfig.FromEmail, _emailConfig.FromName);
            
            message.To.Add(to);
            message.Subject = subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.Body = body;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.HeadersEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = isHtml;

            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to send email.", ex);
        }
    }

    private string CreateEmailConfirmationBody(string confirmationLink)
    {
        var safe = WebUtility.HtmlEncode(confirmationLink);
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Email Confirmation</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #007bff; color: white; padding: 20px; text-align: center; }}
        .content {{ padding: 20px; background-color: #f8f9fa; }}
        .button {{ display: inline-block; background-color: #007bff; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Welcome to ReserveHub!</h1>
        </div>
        <div class='content'>
            <h2>Confirm Your Email Address</h2>
            <p>Thank you for registering with ReserveHub. To complete your registration and start using our services, please confirm your email address by clicking the button below:</p>
            
            <div style='text-align: center;'>
                <a href='{safe}' class='button'>Confirm Email Address</a>
            </div>
            
            <p>If the button doesn't work, you can copy and paste this link into your browser:</p>
            <p style='word-break: break-all; background-color: #e9ecef; padding: 10px; border-radius: 3px;'>{safe}</p>
            
            <p><strong>Important:</strong> This link will expire in 24 hours for security reasons.</p>
            
            <p>If you didn't create an account with ReserveHub, please ignore this email.</p>
        </div>
        <div class='footer'>
            <p>© {DateTime.UtcNow.Year} ReserveHub. All rights reserved.</p>
            <p>This is an automated message, please do not reply to this email.</p>
        </div>
    </div>
</body>
</html>";
    }

    private string CreatePasswordResetBody(string resetLink)
    {
        var safe = System.Net.WebUtility.HtmlEncode(resetLink);
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Password Reset</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #dc3545; color: white; padding: 20px; text-align: center; }}
        .content {{ padding: 20px; background-color: #f8f9fa; }}
        .button {{ display: inline-block; background-color: #dc3545; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Password Reset Request</h1>
        </div>
        <div class='content'>
            <h2>Reset Your Password</h2>
            <p>We received a request to reset your password for your ReserveHub account. Click the button below to reset your password:</p>
            
            <div style='text-align: center;'>
                <a href='{safe}' class='button'>Reset Password</a>
            </div>
            
            <p>If the button doesn't work, you can copy and paste this link into your browser:</p>
            <p style='word-break: break-all; background-color: #e9ecef; padding: 10px; border-radius: 3px;'>{safe}</p>
            
            <p><strong>Important:</strong> This link will expire in 1 hour for security reasons.</p>
            
            <p>If you didn't request a password reset, please ignore this email. Your password will remain unchanged.</p>
        </div>
        <div class='footer'>
            <p>© {DateTime.UtcNow.Year} ReserveHub. All rights reserved.</p>
            <p>This is an automated message, please do not reply to this email.</p>
        </div>
    </div>
</body>
</html>";
    }
}
