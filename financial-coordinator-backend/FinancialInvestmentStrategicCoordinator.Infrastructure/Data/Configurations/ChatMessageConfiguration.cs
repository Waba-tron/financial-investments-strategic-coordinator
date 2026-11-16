using FinancialInvestmentStrategicCoordinator.Domain.Entities.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Data.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("ChatMessages");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Role).IsRequired().HasMaxLength(20);
        builder.Property(m => m.Content).IsRequired().HasMaxLength(8000);

        builder.HasIndex(m => m.ConversationId);
    }
}
