namespace FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;

public class ConflictException : AppException
{
    public ConflictException(string message, string errorCode = "CONFLICT")
        : base(message, errorCode, 409)
    {
    }
}
