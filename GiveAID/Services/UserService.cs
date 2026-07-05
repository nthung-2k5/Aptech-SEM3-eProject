using GiveAID.Data;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class UserService(AppDbContext dbContext) : IUserService
{
    public async Task<List<User>> GetAllUsersAsync(CancellationToken ct) => await dbContext.Users.ToListAsync(ct);

    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId, ct);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken ct = default)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken ct = default)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(ct);
        return user;
    }

    public async Task<User> UpdateUserAsync(User user, CancellationToken ct = default)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(ct);
        return user;
    }

    public async Task<bool> DeleteUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId, ct);

        if (user != null)
        {
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync(ct);
            return true;
        }

        return false;
    }
}
