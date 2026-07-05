using GiveAID.Data;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class AuthService(AppDbContext dbContext) : IAuthService
{
    private readonly PasswordHasher<User> _userPasswordHasher = new();
    private readonly PasswordHasher<Member> _memberPasswordHasher = new();

    public async Task<LoginResult> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return LoginResult.Fail("Invalid credentials.");
        }

        // 1. Check Admin (Users table) trước
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email, ct);

        if (user != null)
        {
            var verifyResult = _userPasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (verifyResult == PasswordVerificationResult.Success)
            {
                return LoginResult.Ok(AccountRole.Admin, user.UserId);
            }

            return LoginResult.Fail("Invalid credentials.");
        }

        // 2. Nếu không phải Admin, check Member
        var member = await dbContext.Members
            .FirstOrDefaultAsync(m => m.Email == email, ct);

        if (member != null)
        {
            if (!member.IsActive)
            {
                return LoginResult.Fail("This account has been deactivated.");
            }

            var verifyResult = _memberPasswordHasher.VerifyHashedPassword(member, member.PasswordHash, password);
            if (verifyResult == PasswordVerificationResult.Success)
            {
                return LoginResult.Ok(AccountRole.Member, member.MemberId);
            }

            return LoginResult.Fail("Invalid credentials.");
        }

        // Không tìm thấy email ở cả 2 bảng
        return LoginResult.Fail("Invalid credentials.");
    }
}