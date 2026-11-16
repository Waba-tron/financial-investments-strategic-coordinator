using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using FinancialInvestmentStrategicCoordinator.Application.Subscription.Models;
using Microsoft.Extensions.Options;
using Stripe;

namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Payments;

/// <summary>Stripe-backed implementation of <see cref="IStripeService"/>.</summary>
public class StripeService : IStripeService
{
    private readonly StripeOptions _options;
    private readonly StripeClient _client;

    public StripeService(IOptions<StripeOptions> options)
    {
        _options = options.Value;
        _client = new StripeClient(_options.SecretKey);
    }

    public async Task<string> CreateCheckoutSessionAsync(
        CheckoutSessionRequest request, CancellationToken cancellationToken = default)
    {
        var options = new Stripe.Checkout.SessionCreateOptions
        {
            Mode = "subscription",
            LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
            {
                new() { Price = _options.ProPriceId, Quantity = 1 }
            },
            SuccessUrl = _options.SuccessUrl,
            CancelUrl = _options.CancelUrl,
            // Round-trips our user id so the webhook can map the session back to a user.
            ClientReferenceId = request.UserId.ToString()
        };

        // Reuse the existing Stripe customer if we have one; otherwise let Checkout create it.
        if (!string.IsNullOrEmpty(request.ExistingCustomerId))
        {
            options.Customer = request.ExistingCustomerId;
        }
        else
        {
            options.CustomerEmail = request.Email;
        }

        var service = new Stripe.Checkout.SessionService(_client);
        var session = await service.CreateAsync(options, cancellationToken: cancellationToken);
        return session.Url;
    }

    public async Task<string> CreateBillingPortalSessionAsync(
        string stripeCustomerId, CancellationToken cancellationToken = default)
    {
        var options = new Stripe.BillingPortal.SessionCreateOptions
        {
            Customer = stripeCustomerId,
            ReturnUrl = _options.BillingPortalReturnUrl
        };

        var service = new Stripe.BillingPortal.SessionService(_client);
        var session = await service.CreateAsync(options, cancellationToken: cancellationToken);
        return session.Url;
    }

    public StripeWebhookEvent? ParseWebhookEvent(string json, string signatureHeader)
    {
        Event stripeEvent;
        try
        {
            // throwOnApiVersionMismatch: false — the account may send events on an older
            // API version than this SDK pins to. The signature is still verified; the
            // fields we read (customer/subscription/client_reference_id) are stable, so a
            // version difference must not be treated as a signature failure.
            stripeEvent = EventUtility.ConstructEvent(
                json, signatureHeader, _options.WebhookSecret, throwOnApiVersionMismatch: false);
        }
        catch (StripeException)
        {
            throw new WebhookSignatureException();
        }

        switch (stripeEvent.Type)
        {
            // Stable Stripe event-type strings (SDK-version-independent).
            case "checkout.session.completed":
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                return new StripeWebhookEvent
                {
                    Type = StripeWebhookEventType.SubscriptionActivated,
                    CustomerId = session?.CustomerId,
                    SubscriptionId = session?.SubscriptionId,
                    ClientReferenceId = session?.ClientReferenceId
                };
            }

            case "customer.subscription.deleted":
            {
                var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                return new StripeWebhookEvent
                {
                    Type = StripeWebhookEventType.SubscriptionCancelled,
                    CustomerId = subscription?.CustomerId,
                    SubscriptionId = subscription?.Id
                };
            }

            default:
                return null; // not an event we act on
        }
    }
}
