using FinancialInvestmentStrategicCoordinator.Domain.Common.Constants;

namespace FinancialInvestmentStrategicCoordinator.Domain.Entities.Auth;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Role { get; set; } = RoleConstants.DefaultRole;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Subscription / usage-limiting.
    public string Plan { get; set; } = PlanConstants.DefaultPlan;   // "Free" | "Pro"
    public int ChatCallsUsed { get; set; } = 0;                     // lifetime counter
    public DateTime? SubscribedAt { get; set; }

    // Stripe linkage (set when the user first checks out / a subscription activates).
    public string? StripeCustomerId { get; set; }
    public string? StripeSubscriptionId { get; set; }
}
