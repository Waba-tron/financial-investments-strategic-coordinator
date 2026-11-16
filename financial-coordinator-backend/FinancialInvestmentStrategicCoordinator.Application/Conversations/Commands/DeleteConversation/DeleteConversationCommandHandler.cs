using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialInvestmentStrategicCoordinator.Application.Conversations.Commands.DeleteConversation;

public class DeleteConversationCommandHandler : IRequestHandler<DeleteConversationCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteConversationCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetRequiredUserId();

        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == userId, cancellationToken)
            ?? throw new ResourceNotFoundException("Conversation", request.Id);

        _context.Conversations.Remove(conversation);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
