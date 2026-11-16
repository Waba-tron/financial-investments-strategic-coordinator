namespace FinancialInvestmentStrategicCoordinator.Application.Subscription.Dtos;

/// <summary>A hosted Stripe URL (Checkout or Billing Portal) for the client to redirect to.</summary>
public class StripeUrlResponse
{
    public string Url { get; set; } = string.Empty;
}
