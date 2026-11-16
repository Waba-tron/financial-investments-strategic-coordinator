namespace FinancialInvestmentStrategicCoordinator.Application.Conversations.Dtos;

/// <summary>Lightweight row for the chat sidebar list.</summary>
public class ConversationSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime UpdatedDate { get; set; }
}
