using FinancialInvestmentStrategicCoordinator.Application.Chat.Commands.SendChatMessage;
using FinancialInvestmentStrategicCoordinator.Application.Chat.Dtos;
using FinancialInvestmentStrategicCoordinator.Application.Chat.Queries.GetUserUsage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialInvestmentStrategicCoordinator.API.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> SendChatMessage(SendChatMessageCommand sendChatMessageCommand, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(sendChatMessageCommand, cancellationToken);
        return Ok(result);
    }

    [HttpGet("usage")]
    public async Task<ActionResult<UserUsageResponse>> GetUserUsage(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserUsageQuery(), cancellationToken);
        return Ok(result);
    }
}
