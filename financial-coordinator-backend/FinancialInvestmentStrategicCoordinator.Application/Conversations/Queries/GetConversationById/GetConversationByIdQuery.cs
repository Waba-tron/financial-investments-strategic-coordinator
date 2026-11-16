using FinancialInvestmentStrategicCoordinator.Application.Conversations.Dtos;
using MediatR;

namespace FinancialInvestmentStrategicCoordinator.Application.Conversations.Queries.GetConversationById;

public record GetConversationByIdQuery(int Id) : IRequest<ConversationDetailDto>;
