using FinancialInvestmentStrategicCoordinator.Application.Chat.Dtos;
using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using FinancialInvestmentStrategicCoordinator.Domain.Common.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialInvestmentStrategicCoordinator.Application.Chat.Queries.GetUserUsage;

public class GetUserUsageQueryHandler : IRequestHandler<GetUserUsageQuery, UserUsageResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GetUserUsageQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<UserUsageResponse> Handle(GetUserUsageQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetRequiredUserId();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new UnauthorizedException();

        var isSubscribed = user.Plan == PlanConstants.Pro;

        return new UserUsageResponse
        {
            Plan = user.Plan,
            CallsUsed = user.ChatCallsUsed,
            CallsRemaining = isSubscribed
                ? null
                : Math.Max(0, PlanConstants.FreeCallLimit - user.ChatCallsUsed),
        };
    }
}
