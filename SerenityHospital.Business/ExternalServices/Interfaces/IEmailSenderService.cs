namespace SerenityHospital.Business.ExternalServices.Interfaces;

public interface IEmailSenderService
{
    Task SendEmailAsync(string fromAddress, string toAddress, string subject, string message);
}

