using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FinancialInvestmentStrategicCoordinator.Infrastructure.Auth;

/// <summary>
/// Resolves the current user id from the JWT "sub" claim on the active request.
/// </summary>
public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

    public int? UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            
            if (user is null)
            {
                return null;
            }

            // NameClaimType is configured as "sub"; also check the raw "sub" claim for safety.
            var value = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? user.FindFirstValue("sub");

            return int.TryParse(value, out var id) ? id : null;
        }
    }
}
