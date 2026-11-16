using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;

namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Chat;

/// <summary>
/// Placeholder chat bot. Returns a canned reply so the limiter and endpoints work
/// end-to-end without an LLM provider or API key. Replace with a real provider
/// (e.g. an HttpClient-based Anthropic/OpenAI client) behind the same interface.
/// </summary>
public class StubChatService : IChatService
{
    public Task<string> GetReplyAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var reply =
            "This is a placeholder financial assistant. You asked: \"" + prompt + "\". " +
            "Once a real LLM is wired up, a proper answer will appear here.";

        return Task.FromResult(reply);
    }
}
