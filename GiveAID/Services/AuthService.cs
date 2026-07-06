using GiveAID.Data;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using GiveAID.Dtos;

namespace GiveAID.Services;

public class AuthService(AppDbContext dbContext, IPasswordService passwordService, IConfiguration configuration) : IAuthService
{
    public async Task<LoginResultDto> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new LoginException("Invalid credentials.");
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

        if (user != null)
        {
            if (user.IsDeleted)
            {
                throw new LoginException("This account has been deactivated.");
            }

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
        var secretKey = jwtSettings["Secret"] ?? "super_secret_default_key_replace_me_in_production_over_32_chars";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"] ?? "GiveAID",
            audience: jwtSettings["Audience"] ?? "GiveAID",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginException(string message) : Exception(message);