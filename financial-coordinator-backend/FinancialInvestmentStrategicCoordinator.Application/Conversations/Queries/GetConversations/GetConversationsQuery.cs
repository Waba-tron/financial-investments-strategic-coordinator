using FinancialInvestmentStrategicCoordinator.Application.Conversations.Dtos;
using MediatR;

namespace FinancialInvestmentStrategicCoordinator.Application.Conversations.Queries.GetConversations;

public record GetConversationsQuery() : IRequest<IReadOnlyList<ConversationSummaryDto>>;
