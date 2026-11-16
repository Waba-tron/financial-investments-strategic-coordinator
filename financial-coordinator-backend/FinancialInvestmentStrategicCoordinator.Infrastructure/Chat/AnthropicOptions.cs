namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Chat;

/// <summary>Configuration for the Anthropic (Claude) chat provider.</summary>
public class AnthropicOptions
{
    public const string SectionName = "Anthropic";

    /// <summary>
    /// Model id. Defaults to the latest Opus. To cut cost, set this to "claude-haiku-5-5"
    /// (~40x cheaper) or "claude-sonnet-5-5" — no code change needed.
    /// </summary>
    public string Model { get; set; } = "claude-opus-5-5";

    /// <summary>Hard cap on reply length. Directly bounds the output-token cost of each call.</summary>
    public int MaxTokens { get; set; } = 1024;

    /// <summary>System prompt that frames the assistant and keeps it out of regulated-advice territory.</summary>
    public string SystemPrompt { get; set; } =
        "You are a concise financial information assistant for a personal investing app. " +
        "Explain concepts, markets, and strategies clearly and briefly. You do not know the " +
        "user's full financial situation, so keep guidance general and remind them to consult " +
        "a licensed financial adviser before making decisions. Never promise or guarantee returns.";
}
