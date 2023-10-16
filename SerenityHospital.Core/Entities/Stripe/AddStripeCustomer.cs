namespace SerenityHospital.Core.Entities.Stripe;

public record AddStripeCustomer(
    string Email,
    string Name,
    AddStripeCard CreditCard
);