using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WorkFromHome.Application.Common.Repository;

namespace WFH.infrastructure.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                SmtpClient client = new SmtpClient
                {
                    Port = 587,
                    Host = _configuration["Smtp:Host"]!,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_configuration["Smtp:Email"], _configuration["Smtp:Password"])
                };

                await client.SendMailAsync(_configuration["Smtp:Email"]!, toEmail, subject, message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
