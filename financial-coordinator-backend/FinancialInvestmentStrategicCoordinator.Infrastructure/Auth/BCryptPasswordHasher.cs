using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;

namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Auth;

public class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string hash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            // Stored hash is malformed — treat as a failed verification rather than throwing.
            return false;
        }
    }
}
