namespace FinancialInvestmentStrategicCoordinator.Domain.Entities.Chat;

/// <summary>
/// A single turn in a <see cref="Conversation"/>. Role is "user" or "assistant"
/// (see ChatRoleConstants).
/// </summary>
public class ChatMessage
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Conversation? Conversation { get; set; }
}
