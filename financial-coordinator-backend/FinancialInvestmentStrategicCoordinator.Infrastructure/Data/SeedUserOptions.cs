namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Data;

public class SeedUserOptions
{
    public const string SectionName = "SeedUsers";

    public List<SeedUser> Users { get; set; } = new();
}

public class SeedUser
{
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
