namespace FinancialInvestmentStrategicCoordinator.Application.Chat.Dtos;

public class ChatResponse
{
    public string Reply { get; set; } = string.Empty;
    public int CallsUsed { get; set; }
    public int? CallsRemaining { get; set; }

    public int ConversationId { get; set; }

    public string Title { get; set; } = string.Empty;
}
