using FinancialInvestmentStrategicCoordinator.Application.Subscription.Commands.CreateStripeBillingPortalSession;
using FinancialInvestmentStrategicCoordinator.Application.Subscription.Commands.CreateStripeCheckoutSession;
using FinancialInvestmentStrategicCoordinator.Application.Subscription.Dtos;
using FinancialInvestmentStrategicCoordinator.Application.Subscription.Queries.GetSubscriptionStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialInvestmentStrategicCoordinator.API.Controllers;

[ApiController]
[Route("api/subscription")]
[Authorize]
public class SubscriptionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<SubscriptionResponse>> GetSubscriptionStatus(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetSubscriptionStatusQuery(), ct);
        return Ok(result);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<StripeUrlResponse>> CreateStripeCheckoutSession(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateStripeCheckoutSessionCommand(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("portal")]
    public async Task<ActionResult<StripeUrlResponse>> CreateStripeBillingPortalSession(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateStripeBillingPortalSessionCommand(), cancellationToken);
        return Ok(result);
    }
}
