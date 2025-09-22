using LMSCourse.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace LMSCourse.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _smtpServer = configuration["Email:SmtpServer"];
            _smtpPort = int.Parse(configuration["Email:SmtpPort"]);
            _smtpUser = configuration["Email:SmtpUser"];
            _smtpPass = configuration["Email:SmtpPass"];
            _logger = logger;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            int retry = 3;
            while (retry > 0)
            {
                try
                {
                    using var client = new SmtpClient(_smtpServer, _smtpPort)
                    {
                        Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                        EnableSsl = true
                    };

                    using var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpUser, "LMS Course"),
                        Subject = subject,
                        Body = htmlMessage,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);
                    break; // gửi thành công
                }
                catch (Exception ex)
                {
                    retry--;
                    _logger.LogError(ex, "Failed to send email to {Email}, retries left: {Retry}", toEmail, retry);
                    if (retry == 0) throw;
                    await Task.Delay(2000); // chờ 2s rồi retry
                }
            }
        }
    }
}
