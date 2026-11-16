using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using FinancialInvestmentStrategicCoordinator.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Data;

/// <summary>
/// Ensures the database exists and creates the configured seed users if they are missing.
/// Passwords are BCrypt-hashed at runtime (so this cannot be done with EF HasData).
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;

        var context = provider.GetRequiredService<ApplicationDbContext>();
        var hasher = provider.GetRequiredService<IPasswordHasher>();
        var seedOptions = provider.GetRequiredService<IOptions<SeedUserOptions>>().Value;
        var environment = provider.GetRequiredService<IHostEnvironment>();
        var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(DatabaseSeeder));

        // In Development, drop the database on every startup so each run begins from a
        // clean, freshly seeded state. Guarded to Development so this can never wipe a
        // real database.
        if (environment.IsDevelopment())
        {
            logger.LogWarning("Development environment: resetting the database on startup.");
            await context.Database.EnsureDeletedAsync(cancellationToken);
        }

        // Dev strategy: EnsureCreated. Swap for context.Database.MigrateAsync() when using migrations.
        await context.Database.EnsureCreatedAsync(cancellationToken);

        foreach (var seed in seedOptions.Users)
        {
            var email = seed.Email.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email))
            {
                continue;
            }

            var exists = await context.Users.AnyAsync(u => u.Email == email, cancellationToken);
            if (exists)
            {
                continue;
            }

            context.Users.Add(new User
            {
                Email = email,
                DisplayName = seed.DisplayName,
                Role = seed.Role,
                PasswordHash = hasher.Hash(seed.Password),
                CreatedDate = DateTime.UtcNow
            });

            logger.LogInformation("Seeded user {Email} with role {Role}.", email, seed.Role);
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
