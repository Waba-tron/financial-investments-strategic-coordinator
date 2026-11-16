using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using FinancialInvestmentStrategicCoordinator.Application.Subscription.Dtos;
using FinancialInvestmentStrategicCoordinator.Application.Subscription.Models;
using FinancialInvestmentStrategicCoordinator.Domain.Common.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialInvestmentStrategicCoordinator.Application.Subscription.Commands.CreateStripeCheckoutSession;

public class CreateStripeCheckoutSessionCommandHandler : IRequestHandler<CreateStripeCheckoutSessionCommand, StripeUrlResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IStripeService _stripe;

    public CreateStripeCheckoutSessionCommandHandler(
        IApplicationDbContext context, ICurrentUser currentUser, IStripeService stripe)
    {
        _context = context;
        _currentUser = currentUser;
        _stripe = stripe;
    }

    public async Task<StripeUrlResponse> Handle(CreateStripeCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetRequiredUserId();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new UnauthorizedException();

        if (user.Plan == PlanConstants.Pro)
        {
            throw new ConflictException("You are already subscribed to Pro.", "ALREADY_SUBSCRIBED");
        }

        var url = await _stripe.CreateCheckoutSessionAsync(new CheckoutSessionRequest
        {
            UserId = user.Id,
            Email = user.Email,
            ExistingCustomerId = user.StripeCustomerId
        }, cancellationToken);

        return new StripeUrlResponse { Url = url };
    }
}
