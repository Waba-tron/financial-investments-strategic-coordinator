using FinancialInvestmentStrategicCoordinator.Domain.Common.Constants;
using FinancialInvestmentStrategicCoordinator.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.DisplayName).IsRequired().HasMaxLength(200);
        builder.Property(u => u.Role).IsRequired().HasMaxLength(50);

        builder.Property(u => u.Plan).IsRequired().HasMaxLength(50).HasDefaultValue(PlanConstants.DefaultPlan);
        builder.Property(u => u.ChatCallsUsed).HasDefaultValue(0);

        builder.Property(u => u.StripeCustomerId).HasMaxLength(255);
        builder.Property(u => u.StripeSubscriptionId).HasMaxLength(255);

        builder.HasIndex(u => u.Email).IsUnique();
        // Webhooks look users up by their Stripe customer id.
        builder.HasIndex(u => u.StripeCustomerId);
    }
}
