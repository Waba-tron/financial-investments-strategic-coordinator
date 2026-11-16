namespace FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;

/// <summary>
/// Base class for all application exceptions. Carries an HTTP status code and a
/// stable machine-readable error code that the global middleware surfaces to clients.
/// </summary>
public class AppException : Exception
{
    public string ErrorCode { get; }
    public int StatusCode { get; }

    public AppException(string message, string errorCode, int statusCode)
        : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
}
