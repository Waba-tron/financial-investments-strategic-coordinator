using MediatR;

namespace FinancialInvestmentStrategicCoordinator.Application.Conversations.Commands.DeleteConversation;

public record DeleteConversationCommand(int Id) : IRequest;
