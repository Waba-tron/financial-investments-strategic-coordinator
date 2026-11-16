namespace FinancialInvestmentStrategicCoordinator.Domain.Common.Constants;

public static class RoleConstants
{
    public const string Customer = "Customer";

    /// <summary>The role assigned to users who self-register.</summary>
    public const string DefaultRole = Customer;

    public static readonly string[] All = { Customer };
}
