namespace FinancialInvestmentStrategicCoordinator.Application.Chat.Dtos;

public class UserUsageResponse
{
    public string Plan { get; set; } = string.Empty;
    public int CallsUsed { get; set; }

    public int? CallsRemaining { get; set; }
}
