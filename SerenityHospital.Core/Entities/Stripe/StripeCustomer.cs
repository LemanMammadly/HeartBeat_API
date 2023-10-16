namespace SerenityHospital.Core.Entities.Stripe;

public record StripeCustomer(
    string Name,
    string Email,
    string CustomerId);