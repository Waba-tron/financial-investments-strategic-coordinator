using Anthropic;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using FinancialInvestmentStrategicCoordinator.Infrastructure.Auth;
using FinancialInvestmentStrategicCoordinator.Infrastructure.Chat;
using FinancialInvestmentStrategicCoordinator.Infrastructure.Data;
using FinancialInvestmentStrategicCoordinator.Infrastructure.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialInvestmentStrategicCoordinator.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Data Source=FinancialInvestmentStrategicCoordinator.db";

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<SeedUserOptions>(options =>
            configuration.GetSection(SeedUserOptions.SectionName).Bind(options.Users));

        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();

        // Current-user accessor (reads the "sub" claim from the active request).
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        // Chat bot. If an Anthropic API key is available, use the real Claude service; otherwise
        // fall back to the offline stub so the app still runs with no key and no cost.
        // Key precedence: ANTHROPIC_API_KEY env var first, then appsettings / user-secrets.
        services.Configure<AnthropicOptions>(configuration.GetSection(AnthropicOptions.SectionName));

        var anthropicApiKey = Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY");
        if (string.IsNullOrWhiteSpace(anthropicApiKey))
        {
            services.AddScoped<IChatService, StubChatService>();
        }
        else
        {
            services.AddSingleton(new AnthropicClient { ApiKey = anthropicApiKey });
            services.AddScoped<IChatService, ClaudeChatService>();
        }

        // Stripe payments / subscriptions.
        services.Configure<StripeOptions>(configuration.GetSection(StripeOptions.SectionName));
        services.AddScoped<IStripeService, StripeService>();

        return services;
    }
}
