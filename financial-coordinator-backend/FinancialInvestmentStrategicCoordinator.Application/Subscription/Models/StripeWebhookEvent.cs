namespace FinancialInvestmentStrategicCoordinator.Application.Subscription.Models;

/// <summary>Provider-agnostic view of the Stripe events we act on.</summary>
public class StripeWebhookEvent
{
    public StripeWebhookEventType Type { get; set; }

    /// <summary>The Stripe customer id (present on subscription + checkout events).</summary>
    public string? CustomerId { get; set; }

    public string? SubscriptionId { get; set; }

    /// <summary>Our user id, round-tripped via Checkout's client_reference_id.</summary>
    public string? ClientReferenceId { get; set; }
}

public enum StripeWebhookEventType
{
    /// <summary>Checkout completed — activate the Pro subscription.</summary>
    SubscriptionActivated,

    /// <summary>Subscription ended/cancelled — downgrade to Free.</summary>
    SubscriptionCancelled
}
