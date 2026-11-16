namespace FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;

/// <summary>
/// Thrown when an incoming webhook fails signature verification. Surfaced as HTTP 400
/// so the provider knows the payload was rejected (and will retry).
/// </summary>
public class WebhookSignatureException : AppException
{
    public WebhookSignatureException(
        string message = "Invalid webhook signature.",
        string errorCode = "INVALID_WEBHOOK_SIGNATURE")
        : base(message, errorCode, 400)
    {
    }
}
