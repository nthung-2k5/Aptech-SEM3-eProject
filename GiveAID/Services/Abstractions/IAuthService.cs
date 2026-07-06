using GiveAID.Dtos;
using GiveAID.Models;

namespace GiveAID.Services.Abstractions;

public interface IAuthService
{
    // Validate credentials against cả Users (Admin) và Members, trả về role để redirect đúng dashboard
    public Task<LoginResultDto> LoginAsync(string email, string password, CancellationToken ct = default);
}