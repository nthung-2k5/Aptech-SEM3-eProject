namespace GiveAID.Models;

public class RegisterResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public Member? Member { get; set; }

    public static RegisterResult Fail(string message) => new() { Success = false, ErrorMessage = message };
    public static RegisterResult Ok(Member member) => new() { Success = true, Member = member };
}