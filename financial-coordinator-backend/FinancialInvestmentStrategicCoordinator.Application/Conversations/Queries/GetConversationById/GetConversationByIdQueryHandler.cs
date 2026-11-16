using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using FinancialInvestmentStrategicCoordinator.Application.Conversations.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialInvestmentStrategicCoordinator.Application.Conversations.Queries.GetConversationById;

public class GetConversationByIdQueryHandler
    : IRequestHandler<GetConversationByIdQuery, ConversationDetailDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GetConversationByIdQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ConversationDetailDto> Handle(
        GetConversationByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetRequiredUserId();

        var conversation = await _context.Conversations
            .Where(c => c.Id == request.Id && c.UserId == userId)
            .Select(c => new ConversationDetailDto
            {
                Id = c.Id,
                Title = c.Title,
                CreatedDate = c.CreatedDate,
                UpdatedDate = c.UpdatedDate,
                Messages = c.Messages
                    .OrderBy(m => m.CreatedDate)
                    .ThenBy(m => m.Id)
                    .Select(m => new MessageDto
                    {
                        Id = m.Id,
                        Role = m.Role,
                        Content = m.Content,
                        CreatedDate = m.CreatedDate
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ResourceNotFoundException("Conversation", request.Id);

        return conversation;
    }
}
