namespace FinancialInvestmentStrategicCoordinator.Domain.Entities.Chat;

/// <summary>
/// A saved chat thread belonging to a single user. Messages are ordered by CreatedDate.
/// </summary>
public class Conversation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    public List<ChatMessage> Messages { get; set; } = new();
}
