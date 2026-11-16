using FinancialInvestmentStrategicCoordinator.Application.Subscription.Models;

namespace FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;

/// <summary>
/// Wraps the Stripe SDK so the Application layer stays free of Stripe types. The
/// implementation lives in Infrastructure.
/// </summary>
public interface IStripeService
{
    /// <summary>Creates a Checkout Session for the Pro subscription and returns its hosted URL.</summary>
    Task<string> CreateCheckoutSessionAsync(CheckoutSessionRequest request, CancellationToken cancellationToken = default);

    /// <summary>Creates a Billing Portal session (manage/cancel) and returns its hosted URL.</summary>
    Task<string> CreateBillingPortalSessionAsync(string stripeCustomerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies the webhook signature and maps the event to a provider-agnostic shape.
    /// Returns null for event types we don't act on. Throws
    /// <see cref="Common.Exceptions.WebhookSignatureException"/> if the signature is invalid.
    /// </summary>
    StripeWebhookEvent? ParseWebhookEvent(string json, string signatureHeader);
}
