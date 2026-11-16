using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using FinancialInvestmentStrategicCoordinator.Application.Subscription.Models;
using FinancialInvestmentStrategicCoordinator.Domain.Common.Constants;
using FinancialInvestmentStrategicCoordinator.Domain.Entities.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinancialInvestmentStrategicCoordinator.Application.Subscription.Commands.HandleStripeWebhook;

public class HandleStripeWebhookCommandHandler : IRequestHandler<HandleStripeWebhookCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IStripeService _stripe;
    private readonly ILogger<HandleStripeWebhookCommandHandler> _logger;

    public HandleStripeWebhookCommandHandler(
        IApplicationDbContext context,
        IStripeService stripe,
        ILogger<HandleStripeWebhookCommandHandler> logger)
    {
        _context = context;
        _stripe = stripe;
        _logger = logger;
    }

    public async Task Handle(HandleStripeWebhookCommand request, CancellationToken cancellationToken)
    {
        // Throws WebhookSignatureException (-> 400) if the signature is invalid;
        // returns null for event types we don't act on.
        var stripeEvent = _stripe.ParseWebhookEvent(request.Json, request.SignatureHeader);
        if (stripeEvent is null)
        {
            return;
        }

        switch (stripeEvent.Type)
        {
            case StripeWebhookEventType.SubscriptionActivated:
                await ActivateAsync(stripeEvent, cancellationToken);
                break;

            case StripeWebhookEventType.SubscriptionCancelled:
                await CancelAsync(stripeEvent, cancellationToken);
                break;
        }
    }

    private async Task ActivateAsync(StripeWebhookEvent evt, CancellationToken cancellationToken)
    {
        // Activation (checkout.session.completed) carries client_reference_id — our user id.
        var user = await FindUserByClientReferenceAsync(evt, cancellationToken);
        if (user is null)
        {
            // A payment succeeded but we can't attribute it to a user. This is permanent
            // (retrying won't find them), so we don't throw — but log at Error so it's
            // surfaced for manual reconciliation rather than silently lost.
            _logger.LogError(
                "Stripe activation for unknown user (customer {CustomerId}, ref {Ref}). " +
                "Payment succeeded but no matching user — needs manual reconciliation.",
                evt.CustomerId, evt.ClientReferenceId);
                
            return;
        }

        user.Plan = PlanConstants.Pro;
        user.SubscribedAt = DateTime.UtcNow;
        user.StripeCustomerId = evt.CustomerId ?? user.StripeCustomerId;
        user.StripeSubscriptionId = evt.SubscriptionId ?? user.StripeSubscriptionId;

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Activated Pro for user {UserId}.", user.Id);
    }

    private async Task CancelAsync(StripeWebhookEvent evt, CancellationToken cancellationToken)
    {
        // Cancellation (customer.subscription.deleted) has no client_reference_id — only the customer id.
        var user = await FindUserByStripeCustomerAsync(evt, cancellationToken);
        if (user is null)
        {
            _logger.LogError("Stripe cancellation for unknown customer {CustomerId}.", evt.CustomerId);
            return;
        }

        user.Plan = PlanConstants.Free;
        user.SubscribedAt = null;
        user.StripeSubscriptionId = null;

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Downgraded user {UserId} to Free.", user.Id);
    }

    /// <summary>Finds the user by the id we stamped on the checkout session's client_reference_id.</summary>
    private async Task<User?> FindUserByClientReferenceAsync(StripeWebhookEvent evt, CancellationToken cancellationToken)
    {
        return int.TryParse(evt.ClientReferenceId, out var userId)
            ? await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            : null;
    }

    /// <summary>Finds the user by their stored Stripe customer id. Guards the null — otherwise it matches users with no customer id.</summary>
    private async Task<User?> FindUserByStripeCustomerAsync(StripeWebhookEvent evt, CancellationToken cancellationToken)
    {
        return string.IsNullOrEmpty(evt.CustomerId)
            ? null
            : await _context.Users.FirstOrDefaultAsync(u => u.StripeCustomerId == evt.CustomerId, cancellationToken);
    }
}
