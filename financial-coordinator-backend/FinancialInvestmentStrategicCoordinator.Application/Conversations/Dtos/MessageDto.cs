namespace FinancialInvestmentStrategicCoordinator.Application.Conversations.Dtos;

public class MessageDto
{
    public int Id { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
