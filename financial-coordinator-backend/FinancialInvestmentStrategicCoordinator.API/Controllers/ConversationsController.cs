using FinancialInvestmentStrategicCoordinator.Application.Conversations.Commands.DeleteConversation;
using FinancialInvestmentStrategicCoordinator.Application.Conversations.Dtos;
using FinancialInvestmentStrategicCoordinator.Application.Conversations.Queries.GetConversationById;
using FinancialInvestmentStrategicCoordinator.Application.Conversations.Queries.GetConversations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialInvestmentStrategicCoordinator.API.Controllers;

[ApiController]
[Route("api/conversations")]
[Authorize]
public class ConversationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConversationsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ConversationSummaryDto>> GetAllChatConversations(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetConversationsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ConversationDetailDto>> GetConversationById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetConversationByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteConversationById(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteConversationCommand(id), cancellationToken);
        return NoContent();
    }
}
