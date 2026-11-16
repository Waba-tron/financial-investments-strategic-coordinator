using MediatR;

namespace FinancialInvestmentStrategicCoordinator.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;
