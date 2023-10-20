using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.ExternalServices.Interfaces;

public interface IEmailServiceSender
{
    void SendEmail(Message message);
}

