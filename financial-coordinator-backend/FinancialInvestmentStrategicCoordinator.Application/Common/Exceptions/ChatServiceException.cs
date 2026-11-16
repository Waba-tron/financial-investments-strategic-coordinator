namespace FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;

/// <summary>
/// Thrown when the upstream chat/LLM provider fails or returns nothing usable.
/// Surfaced as HTTP 502 Bad Gateway by the global exception middleware so the
/// client can tell "the AI is temporarily unavailable" apart from a real 500.
/// </summary>
public class ChatServiceException : AppException
{
    public ChatServiceException(
        string message = "The assistant is temporarily unavailable. Please try again.",
        string errorCode = "CHAT_SERVICE_UNAVAILABLE")
        : base(message, errorCode, 502)
    {
    }
}
