using Confab.Modules.Users.Core.DTO;
using Confab.Modules.Users.Core.Entities;
using Confab.Modules.Users.Core.Events;
using Confab.Modules.Users.Core.Exceptions;
using Confab.Modules.Users.Core.Repositories;
using Confab.Shared.Abstractions;
using Confab.Shared.Abstractions.Auth;
using Confab.Shared.Abstractions.Messaging;
using Microsoft.AspNetCore.Identity;

namespace Confab.Modules.Users.Core.Services;

internal class IdentityService(
    IUserRepository userRepository,
    IPasswordHasher<User> passwordHasher,
    IAuthManager authManager,
    IClock clock,
    IMessageBroker messageBroker)
    : IIdentityService
{
    
    public async Task<AccountDto?> GetAsync(Guid id)
    {
        var user = await userRepository.GetAsync(id);

        return user is null
            ? null
            : new AccountDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Claims = user.Claims,
                CreatedAt = user.CreatedAt
            };
    }

    public async Task SignUpAsync(SignUpDto dto)
    {
        dto.Id = Guid.NewGuid();
        var email = dto.Email.ToLowerInvariant();
        var user = await userRepository.GetAsync(email);
        if (user is not null)
            throw new EmailInUseException();

        var password = passwordHasher.HashPassword(default, dto.Password);
        user = new User
        {
            Id = dto.Id,
            Email = dto.Email,
            Password = password,
            Role = dto.Role?.ToLowerInvariant() ?? "user",
            CreatedAt = clock.CurrentDate(),
            IsActive = true,
            Claims = dto.Claims
        };
        await userRepository.AddAsync(user);
        await messageBroker.PublishAsync(new SignedUp(user.Id, user.Email));
    }

    public async Task<JsonWebToken> SignInAsync(SignInDto dto)
    {
        var user = await userRepository.GetAsync(dto.Email.ToLowerInvariant())
            ?? throw new InvalidCredentialsException();

        if (passwordHasher.VerifyHashedPassword(default, user.Password, dto.Password) ==
            PasswordVerificationResult.Failed)
            throw new InvalidCredentialsException();

        if (!user.IsActive)
            throw new UserNotActiveException(user.Id);

        var jwt = authManager.CreateToken(user.Id.ToString(), user.Role, claims: user.Claims);
        jwt.Email = user.Email;
        messageBroker.PublishAsync(new SignedIn(user.Id));
        
        return jwt;
    }
}