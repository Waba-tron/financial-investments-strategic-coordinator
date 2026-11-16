using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using FinancialInvestmentStrategicCoordinator.Application.Subscription.Dtos;
using FinancialInvestmentStrategicCoordinator.Domain.Common.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialInvestmentStrategicCoordinator.Application.Subscription.Queries.GetSubscriptionStatus;

public class GetSubscriptionStatusQueryHandler : IRequestHandler<GetSubscriptionStatusQuery, SubscriptionResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GetSubscriptionStatusQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<SubscriptionResponse> Handle(GetSubscriptionStatusQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetRequiredUserId();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new UnauthorizedException();

        return new SubscriptionResponse
        {
            Plan = user.Plan,
            IsSubscribed = user.Plan == PlanConstants.Pro,
            SubscribedAt = user.SubscribedAt
        };
    }
}
