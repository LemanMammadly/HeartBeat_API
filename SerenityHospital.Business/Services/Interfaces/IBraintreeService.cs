using Braintree;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IBraintreeService
{
    IBraintreeGateway CreateGateway();
    IBraintreeGateway GetGateway();
}