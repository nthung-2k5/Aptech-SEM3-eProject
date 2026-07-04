using GiveAID.Models;

namespace GiveAID.Services.Abstractions;

public interface IUserService
{
    public Task<List<User>> GetAllUsersAsync(CancellationToken ct = default);

    public Task<User?> GetUserByIdAsync(Guid userId, CancellationToken ct = default);
    public Task<User?> GetUserByEmailAsync(string email, CancellationToken ct = default);

    public Task<User> CreateUserAsync(User user, CancellationToken ct = default);
    public Task<User> UpdateUserAsync(User user, CancellationToken ct = default);

    public Task<bool> DeleteUserAsync(Guid userId, CancellationToken ct = default);
}
