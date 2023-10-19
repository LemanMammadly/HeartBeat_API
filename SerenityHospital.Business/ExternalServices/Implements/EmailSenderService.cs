using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using SerenityHospital.Business.ExternalServices.Interfaces;

namespace SerenityHospital.Business.ExternalServices.Implements;

public class EmailSenderService : IEmailSenderService
{

    readonly IConfiguration _config;

    public EmailSenderService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string fromAddress, string toAddress, string subject, string message)
    {
        var mailMessage = new MailMessage(fromAddress, toAddress, subject, message);


        using (var client = new SmtpClient(_config["Email:Host"], int.Parse(_config["Email:Port"]))
        {
            Credentials = new NetworkCredential(_config["Email:Username"], _config["Email:Password"])
        })
        {
            await client.SendMailAsync(mailMessage);
        }
    }
    
}