using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IMemberService
{
    Task<MemberSummaryDto[]> GetAllMembersAsync(string? searchTerm, int page = 1, int pageSize = 10,
                                                CancellationToken ct = default);

    Task<MemberDto?> GetMemberByIdAsync(Guid id, CancellationToken ct = default);
    Task<MemberDto> CreateMemberAsync(MemberCreateDto dto, CancellationToken ct = default);
    Task UpdateMemberAsync(Guid id, MemberUpdateDto dto, CancellationToken ct = default);
    Task DeleteMemberAsync(Guid id, CancellationToken ct = default);
}
