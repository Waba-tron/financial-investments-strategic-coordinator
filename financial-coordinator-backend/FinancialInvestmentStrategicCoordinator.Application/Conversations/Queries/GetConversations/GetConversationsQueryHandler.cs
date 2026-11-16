using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using FinancialInvestmentStrategicCoordinator.Application.Conversations.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialInvestmentStrategicCoordinator.Application.Conversations.Queries.GetConversations;

public class GetConversationsQueryHandler
    : IRequestHandler<GetConversationsQuery, IReadOnlyList<ConversationSummaryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GetConversationsQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<ConversationSummaryDto>> Handle(
        GetConversationsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetRequiredUserId();

        return await _context.Conversations
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.UpdatedDate)
            .Select(c => new ConversationSummaryDto
            {
                Id = c.Id,
                Title = c.Title,
                UpdatedDate = c.UpdatedDate
            })
            .ToListAsync(cancellationToken);
    }
}
