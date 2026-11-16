using FinancialInvestmentStrategicCoordinator.Application.Chat.Dtos;
using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using FinancialInvestmentStrategicCoordinator.Domain.Common.Constants;
using FinancialInvestmentStrategicCoordinator.Domain.Entities.Chat;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialInvestmentStrategicCoordinator.Application.Chat.Commands.SendChatMessage;

public class SendChatMessageCommandHandler : IRequestHandler<SendChatMessageCommand, ChatResponse>
{
    private const int TitleMaxLength = 60;

    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IChatService _chatService;

    public SendChatMessageCommandHandler(
        IApplicationDbContext context,
        ICurrentUser currentUser,
        IChatService chatService)
    {
        _context = context;
        _currentUser = currentUser;
        _chatService = chatService;
    }

    public async Task<ChatResponse> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetRequiredUserId();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new UnauthorizedException();

        var isSubscribed = user.Plan == PlanConstants.Pro;

        if (!isSubscribed && user.ChatCallsUsed >= PlanConstants.FreeCallLimit)
        {
            throw new QuotaExceededException();
        }

        // Resolve the target conversation: an existing one owned by the user, or a new one.
        Conversation conversation;
        
        if (request.ConversationId is int conversationId)
        {
            conversation = await _context.Conversations
                .FirstOrDefaultAsync(c => c.Id == conversationId && c.UserId == userId, cancellationToken)
                ?? throw new ResourceNotFoundException("Conversation", conversationId);
        }
        else
        {

            conversation = new Conversation
            {
                UserId = userId,
                Title = BuildTitle(request.Message),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            _context.Conversations.Add(conversation);
        }

        var reply = await _chatService.GetReplyAsync(request.Message, cancellationToken);

        var now = DateTime.UtcNow;

        conversation.Messages.Add(new ChatMessage
        {
            Role = ChatRoleConstants.User,
            Content = request.Message,
            CreatedDate = now
        });

        conversation.Messages.Add(new ChatMessage
        {
            Role = ChatRoleConstants.Assistant,
            Content = reply,
            CreatedDate = now.AddTicks(1) // keep the assistant turn strictly after the user turn
        });

        conversation.UpdatedDate = now;

        if (!isSubscribed)
        {
            user.ChatCallsUsed++;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new ChatResponse
        {
            Reply = reply,
            ConversationId = conversation.Id,
            Title = conversation.Title,
            CallsUsed = user.ChatCallsUsed,
            CallsRemaining = isSubscribed
                ? null
                : Math.Max(0, PlanConstants.FreeCallLimit - user.ChatCallsUsed),
        };
    }

    private static string BuildTitle(string prompt)
    {
        var trimmed = prompt.Trim();
        if (trimmed.Length <= TitleMaxLength)
        {
            return trimmed;
        }

        return trimmed[..TitleMaxLength].TrimEnd() + "…";
    }
}
