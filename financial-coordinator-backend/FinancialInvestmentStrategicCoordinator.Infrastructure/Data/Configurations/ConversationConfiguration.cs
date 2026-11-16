using FinancialInvestmentStrategicCoordinator.Domain.Entities.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Data.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("Conversations");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title).IsRequired().HasMaxLength(200);

        builder.HasMany(c => c.Messages)
            .WithOne(m => m.Conversation!)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        // List queries fetch a user's conversations newest-first.
        builder.HasIndex(c => new { c.UserId, c.UpdatedDate });
    }
}
