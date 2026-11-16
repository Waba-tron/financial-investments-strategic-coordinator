using FinancialInvestmentStrategicCoordinator.Application.Chat.Dtos;
using MediatR;

namespace FinancialInvestmentStrategicCoordinator.Application.Chat.Queries.GetUserUsage;

public record GetUserUsageQuery : IRequest<UserUsageResponse>;
