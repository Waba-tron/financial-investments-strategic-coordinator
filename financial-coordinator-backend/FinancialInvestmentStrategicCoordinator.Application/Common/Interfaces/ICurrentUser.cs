using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;

namespace FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;

/// <summary>
/// Resolves the currently authenticated user from the request context.
/// </summary>
public interface ICurrentUser
{
    /// <summary>The authenticated user's id (from the JWT "sub" claim), or null if unauthenticated.</summary>
    int? UserId { get; }

    /// <summary>The authenticated user's id, or throws <see cref="UnauthorizedException"/> if unauthenticated.</summary>
    int GetRequiredUserId() => UserId ?? throw new UnauthorizedException();
}
