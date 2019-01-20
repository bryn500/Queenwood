using Microsoft.Extensions.Options;
using Queenwood.Models;
using Queenwood.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Queenwood.Core.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;

        public EmailService(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task<Result> SendEnquiry(string subject, string body)
        {
            var result = new Result();

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_emailConfig.EmailFrom);
                message.To.Add(_emailConfig.EmailFrom);

                message.Subject = subject;
                message.Body = body;

                await Send(result, message);
            }

            return result;
        }

        public async void SendErrorAlert(string details)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_emailConfig.EmailFrom);
                message.To.Add(_emailConfig.ErrorEmail);

                message.Subject = "Error on " + Consts.BrandName;
                message.Body = details;

                await Send(new Result(), message);
            }
        }

        private async Task Send(Result result, MailMessage message)
        {
            try
            {
                using (var client = new SmtpClient(_emailConfig.SmtpClientHost)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_emailConfig.SmtpUser, _emailConfig.SmtpPassword)
                })
                {
                    await client.SendMailAsync(message);
                    result.IsError = false;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Failed to send message";
                result.IsError = true;
            }
        }
    }
}
