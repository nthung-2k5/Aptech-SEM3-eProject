namespace GiveAID.Models;

public class LoginResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public AccountRole? Role { get; set; }
    public Guid? AccountId { get; set; }

    public static LoginResult Fail(string message) => new() { Success = false, ErrorMessage = message };

    public static LoginResult Ok(AccountRole role, Guid accountId) => new()
    {
        Success = true,
        Role = role,
        AccountId = accountId
    };
}