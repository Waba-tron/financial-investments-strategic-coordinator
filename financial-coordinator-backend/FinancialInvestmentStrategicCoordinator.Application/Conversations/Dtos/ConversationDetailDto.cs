namespace FinancialInvestmentStrategicCoordinator.Application.Conversations.Dtos;

/// <summary>A conversation plus its ordered messages.</summary>
public class ConversationDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public List<MessageDto> Messages { get; set; } = new();
}
