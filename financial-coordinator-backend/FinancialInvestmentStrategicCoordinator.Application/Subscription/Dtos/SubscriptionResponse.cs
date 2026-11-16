namespace FinancialInvestmentStrategicCoordinator.Application.Subscription.Dtos;

public class SubscriptionResponse
{
    public string Plan { get; set; } = string.Empty;
    public bool IsSubscribed { get; set; }
    public DateTime? SubscribedAt { get; set; }
}
