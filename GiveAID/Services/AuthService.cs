using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GiveAID.Services;

public class AuthService(AppDbContext dbContext, IPasswordService passwordService, IConfiguration configuration)
        : IAuthService
{
    public async Task<LoginResultDto> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new LoginException("Invalid credentials.");
        }

        var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, ct);

        if (user != null)
        {
            if (user.IsDeleted) { throw new LoginException("This account has been deactivated."); }

            if (passwordService.VerifyPassword(password, user.PasswordHash))
            {
                string token = GenerateJwtToken(user);
                return new LoginResultDto(user.UserId, user.Role, token);
            }
        }

        throw new LoginException("Invalid credentials.");
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        string secretKey = jwtSettings["Secret"] ?? "super_secret_default_key_replace_me_in_production_over_32_chars";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            jwtSettings["Issuer"] ?? "GiveAID",
            jwtSettings["Audience"] ?? "GiveAID",
            claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
