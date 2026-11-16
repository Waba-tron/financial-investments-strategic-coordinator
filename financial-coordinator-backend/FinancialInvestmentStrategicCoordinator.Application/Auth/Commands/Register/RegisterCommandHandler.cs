using FinancialInvestmentStrategicCoordinator.Application.Common.Exceptions;
using FinancialInvestmentStrategicCoordinator.Application.Common.Interfaces;
using FinancialInvestmentStrategicCoordinator.Domain.Common.Constants;
using FinancialInvestmentStrategicCoordinator.Domain.Entities.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialInvestmentStrategicCoordinator.Application.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var exists = await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
        if (exists)
        {
            throw new ConflictException("Email already registered.", "EMAIL_ALREADY_REGISTERED");
        }

        var user = new User
        {
            Email = email,
            DisplayName = request.DisplayName.Trim(),
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = RoleConstants.DefaultRole,
            CreatedDate = DateTime.UtcNow
        };

        _context.Users.Add(user);
        
        await _context.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenService.CreateToken(user.Id, user.Email, user.Role);

        return new AuthResponse { Token = token, DisplayName = user.DisplayName };
    }
}
