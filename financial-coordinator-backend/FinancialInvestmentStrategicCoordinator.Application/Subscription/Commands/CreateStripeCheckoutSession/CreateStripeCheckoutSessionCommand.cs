using FinancialInvestmentStrategicCoordinator.Application.Subscription.Dtos;
using MediatR;

namespace FinancialInvestmentStrategicCoordinator.Application.Subscription.Commands.CreateStripeCheckoutSession;

/// <summary>Starts a Stripe Checkout for the current user to subscribe to Pro.</summary>
public record CreateStripeCheckoutSessionCommand : IRequest<StripeUrlResponse>;
