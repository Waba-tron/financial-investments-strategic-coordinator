using MediatR;

namespace FinancialInvestmentStrategicCoordinator.Application.Auth.Commands.Register;

public record RegisterCommand(string Email, string Password, string DisplayName) : IRequest<AuthResponse>;
