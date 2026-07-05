using GiveAID.Models;

namespace GiveAID.Services.Abstractions;

public interface IMemberService
{
    public Task<PagedResult<Member>> GetMembersAsync(
        string? searchTerm,
        int page = 1,
        int pageSize = 10,
        CancellationToken ct = default);

    public Task<Member?> GetMemberByIdAsync(Guid memberId, CancellationToken ct = default);

    public Task<Member> CreateMemberAsync(Member member, CancellationToken ct = default);

    public Task<Member> UpdateMemberAsync(Member member, CancellationToken ct = default);

    public Task<bool> DeactivateMemberAsync(Guid memberId, CancellationToken ct = default);

    public Task<RegisterResult> RegisterMemberAsync(Member member, string plainPassword, CancellationToken ct = default);

    // Member tự cập nhật profile của mình - chỉ các field optional
    public Task<UpdateProfileResult> UpdateMemberProfileAsync(
        Guid memberId,
        string? fullName,
        DateTime? dateOfBirth,
        string? address,
        string? occupation,
        CancellationToken ct = default);
}