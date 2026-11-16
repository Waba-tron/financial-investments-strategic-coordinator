namespace FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;

/// <summary>
/// Thrown when a Free-plan user has used up their free chat calls and must subscribe.
/// Surfaced as HTTP 402 Payment Required by the global exception middleware.
/// </summary>
public class QuotaExceededException : AppException
{
    public QuotaExceededException(
        string message = "Free call limit reached. Subscribe to continue.",
        string errorCode = "QUOTA_EXCEEDED")
        : base(message, errorCode, 402)
    {
    }
}
