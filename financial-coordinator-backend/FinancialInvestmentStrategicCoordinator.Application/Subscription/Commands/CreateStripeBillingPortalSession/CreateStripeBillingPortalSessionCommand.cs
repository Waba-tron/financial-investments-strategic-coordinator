using FinancialInvestmentStrategicCoordinator.Application.Subscription.Dtos;
using MediatR;

namespace FinancialInvestmentStrategicCoordinator.Application.Subscription.Commands.CreateStripeBillingPortalSession;

/// <summary>Opens the Stripe Billing Portal so the current user can manage or cancel their plan.</summary>
public record CreateStripeBillingPortalSessionCommand : IRequest<StripeUrlResponse>;
