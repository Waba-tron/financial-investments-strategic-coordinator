namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Payments;

/// <summary>Bound from the "Stripe" configuration section.</summary>
public class StripeOptions
{
    public const string SectionName = "Stripe";

    /// <summary>Secret API key (sk_test_… / sk_live_…).</summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>Signing secret for the webhook endpoint (whsec_…).</summary>
    public string WebhookSecret { get; set; } = string.Empty;

    /// <summary>The recurring Price id for the Pro plan (price_…).</summary>
    public string ProPriceId { get; set; } = string.Empty;

    /// <summary>Where Checkout redirects on success / cancel.</summary>
    public string SuccessUrl { get; set; } = string.Empty;
    public string CancelUrl { get; set; } = string.Empty;

    /// <summary>Where the Billing Portal returns the user.</summary>
    public string BillingPortalReturnUrl { get; set; } = string.Empty;
}
