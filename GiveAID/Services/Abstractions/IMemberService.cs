using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IMemberService
{
    Task<MemberSummaryDto[]> GetAllMembersAsync(string? searchTerm, int page = 1, int pageSize = 10, CancellationToken ct = default);
    Task<MemberDto?> GetMemberByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> CreateMemberAsync(MemberSaveDto member, CancellationToken ct = default);
    Task<bool> UpdateMemberAsync(Guid id, MemberSaveDto member, CancellationToken ct = default);
    Task<bool> DeleteMemberAsync(Guid id, CancellationToken ct = default);
}