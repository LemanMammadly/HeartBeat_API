using SerenityHospital.Core.Entities.Stripe;

namespace SerenityHospital.Business.Contracts;

public interface IStripeAppService
{
    Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct);
    Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken ct);
}