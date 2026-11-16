namespace FinancialInvestmentStrategicCoordinator.Application.Subscription.Models;

/// <summary>Inputs the Stripe service needs to open a Checkout Session for a user.</summary>
public class CheckoutSessionRequest
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;

    /// <summary>Reuse this Stripe customer if the user already has one; otherwise Stripe creates it.</summary>
    public string? ExistingCustomerId { get; set; }
}
