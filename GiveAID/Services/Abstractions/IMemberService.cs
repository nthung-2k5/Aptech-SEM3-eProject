using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IMemberService
{
    Task<MemberSummaryDto[]> GetAllMembersAsync(CancellationToken ct = default);
    Task<MemberDto?> GetMemberByIdAsync(Guid id, CancellationToken ct = default);
    Task CreateMemberAsync(MemberSaveDto member, CancellationToken ct = default);
    Task UpdateMemberAsync(Guid id, MemberSaveDto member, CancellationToken ct = default);
    Task DeleteMemberAsync(Guid id, CancellationToken ct = default);
}
