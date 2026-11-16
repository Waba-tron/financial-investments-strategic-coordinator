namespace FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Unauthorized.", string errorCode = "UNAUTHORIZED")
        : base(message, errorCode, 401)
    {
    }
}
