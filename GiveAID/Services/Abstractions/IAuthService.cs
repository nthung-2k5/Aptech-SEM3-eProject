using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IAuthService
{
    // Validate credentials against Users (Admin) and Members, return appropriate roles for correct dashboard redirection
    public Task<LoginResultDto> LoginAsync(string email, string password, CancellationToken ct = default);
    public Task<string?> RefreshTokenAsync(Guid userId, DateTime? expires = null, CancellationToken ct = default);
}
