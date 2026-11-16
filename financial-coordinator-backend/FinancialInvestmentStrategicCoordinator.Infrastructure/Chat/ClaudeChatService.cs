using Anthropic;
using Anthropic.Models.Messages;
using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Chat;

/// <summary>
/// Real chat bot backed by the Anthropic (Claude) Messages API. Each call is a single
/// stateless request: the system prompt frames the assistant, the user's message is the
/// only turn. Conversation memory, if wanted later, would mean passing prior turns here.
/// </summary>
public class ClaudeChatService : IChatService
{
    private readonly AnthropicClient _anthropicClient;
    private readonly AnthropicOptions _options;
    private readonly ILogger<ClaudeChatService> _logger;

    public ClaudeChatService(
        AnthropicClient anthropicClient,
        IOptions<AnthropicOptions> options,
        ILogger<ClaudeChatService> logger)
    {
        _anthropicClient = anthropicClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> GetReplyAsync(string prompt, CancellationToken cancellationToken = default)
    {
        Message response;
        try
        {
            response = await _anthropicClient.Messages.Create(new MessageCreateParams
            {
                Model = _options.Model,
                MaxTokens = _options.MaxTokens,
                System = _options.SystemPrompt,
                Messages = [new UserMessageParam { Content = prompt }],
            }, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw; // caller cancelled — don't mask it as a service failure
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Claude chat request failed (model {Model}).", _options.Model);
            throw new ChatServiceException();
        }

        // Content is a union of block types; keep only the text blocks and concatenate them.
        var reply = string.Concat(
            response.Content.Select(b => b.Value).OfType<TextBlock>().Select(t => t.Text));

        if (string.IsNullOrWhiteSpace(reply))
        {
            _logger.LogWarning(
                "Claude returned no text content (stop reason {StopReason}).", response.StopReason);
            throw new ChatServiceException();
        }

        return reply;
    }
}
