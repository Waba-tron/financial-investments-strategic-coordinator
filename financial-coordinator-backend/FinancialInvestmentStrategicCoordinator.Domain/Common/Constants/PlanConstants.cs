namespace FinancialInvestmentStrategicCoordinator.Domain.Common.Constants;

public static class PlanConstants
{
    public const string Free = "Free";
    public const string Pro = "Pro";

    /// <summary>The plan assigned to users who self-register.</summary>
    public const string DefaultPlan = Free;

    /// <summary>Number of free chat calls a Free-plan user gets (lifetime).</summary>
    public const int FreeCallLimit = 5;

    public static readonly string[] All = { Free, Pro };
}
