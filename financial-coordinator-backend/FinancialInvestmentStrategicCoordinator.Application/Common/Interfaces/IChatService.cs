namespace FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;

/// <summary>
/// Sends a prompt to the chat bot (LLM) and returns its reply. The concrete
/// implementation lives in Infrastructure; swap it for a real provider without
/// touching the limiter/handler.
/// </summary>
public interface IChatService
{
    Task<string> GetReplyAsync(string prompt, CancellationToken cancellationToken = default);
}
