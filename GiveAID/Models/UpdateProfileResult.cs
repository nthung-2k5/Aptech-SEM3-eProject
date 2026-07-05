namespace GiveAID.Models;

public class UpdateProfileResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public Member? Member { get; set; }

    public static UpdateProfileResult Fail(string message) => new() { Success = false, ErrorMessage = message };
    public static UpdateProfileResult Ok(Member member) => new() { Success = true, Member = member };
}