namespace FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException(string message = "Forbidden.", string errorCode = "FORBIDDEN")
        : base(message, errorCode, 403)
    {
    }
}
