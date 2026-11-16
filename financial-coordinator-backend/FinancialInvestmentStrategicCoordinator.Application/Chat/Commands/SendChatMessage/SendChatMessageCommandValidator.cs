using FluentValidation;

namespace FinancialInvestmentStrategicCoordinator.Application.Chat.Commands.SendChatMessage;

public class SendChatMessageCommandValidator : AbstractValidator<SendChatMessageCommand>
{
    public SendChatMessageCommandValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(4000).WithMessage("Message must be at most 4000 characters.");
    }
}
