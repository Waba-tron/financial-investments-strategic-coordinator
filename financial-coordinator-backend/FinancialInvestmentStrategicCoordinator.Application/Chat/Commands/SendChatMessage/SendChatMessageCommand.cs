using FinancialInvestmentStrategicCoordinator.Application.Chat.Dtos;
using MediatR;

namespace FinancialInvestmentStrategicCoordinator.Application.Chat.Commands.SendChatMessage;

/// <summary>
/// Sends a message to the assistant. When <paramref name="ConversationId"/> is null a new
/// conversation is created (titled from the message); otherwise the message is appended to the
/// existing conversation owned by the current user.
/// </summary>
public record SendChatMessageCommand(string Message, int? ConversationId = null) : IRequest<ChatResponse>;
