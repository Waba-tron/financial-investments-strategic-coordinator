using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using FinancialInvestmentStrategicCoordinator.Application.Subscription.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialInvestmentStrategicCoordinator.Application.Subscription.Commands.CreateStripeBillingPortalSession;

public class CreateStripeBillingPortalSessionCommandHandler
    : IRequestHandler<CreateStripeBillingPortalSessionCommand, StripeUrlResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IStripeService _stripe;

    public CreateStripeBillingPortalSessionCommandHandler(
        IApplicationDbContext context, ICurrentUser currentUser, IStripeService stripe)
    {
        _context = context;
        _currentUser = currentUser;
        _stripe = stripe;
    }

    public async Task<StripeUrlResponse> Handle(CreateStripeBillingPortalSessionCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetRequiredUserId();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new UnauthorizedException();

        if (string.IsNullOrEmpty(user.StripeCustomerId))
        {
            throw new ConflictException("No billing account yet. Subscribe first.", "NO_STRIPE_CUSTOMER");
        }

        var url = await _stripe.CreateBillingPortalSessionAsync(user.StripeCustomerId, cancellationToken);
        return new StripeUrlResponse { Url = url };
    }
}
