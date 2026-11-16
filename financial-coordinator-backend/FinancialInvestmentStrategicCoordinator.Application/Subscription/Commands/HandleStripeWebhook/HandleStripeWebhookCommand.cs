using MediatR;

namespace FinancialInvestmentStrategicCoordinator.Application.Subscription.Commands.HandleStripeWebhook;

/// <summary>
/// Processes a raw Stripe webhook. The handler verifies the signature (via IStripeService)
/// and applies the resulting plan change. Carries the raw payload + signature header.
/// </summary>
public record HandleStripeWebhookCommand(string Json, string SignatureHeader) : IRequest;
