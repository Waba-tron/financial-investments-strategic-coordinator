using FinancialInvestmentStrategicCoordinator.Application.Subscription.Commands.HandleStripeWebhook;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialInvestmentStrategicCoordinator.API.Controllers;

/// <summary>
/// Receives Stripe webhooks. Unauthenticated by design — Stripe can't send a JWT; the
/// request is instead authenticated by its signature, verified inside the handler.
/// </summary>
/// 
[ApiController]
[Route("api/webhooks/stripe")]
[AllowAnonymous]
public class StripeWebhookController : ControllerBase
{
    private readonly IMediator _mediator;

    public StripeWebhookController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Handle(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body);
        var json = await reader.ReadToEndAsync(cancellationToken);
        var signature = Request.Headers["Stripe-Signature"].ToString();

        await _mediator.Send(new HandleStripeWebhookCommand(json, signature), cancellationToken);
        return Ok();
    }
}
