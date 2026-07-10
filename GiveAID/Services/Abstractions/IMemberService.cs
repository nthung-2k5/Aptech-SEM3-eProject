using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IMemberService
{
    Task<PagedResult<MemberDto>> GetMembersPagedAsync(MemberQueryParameters query, CancellationToken ct = default);

    Task<MemberSummaryDto[]> GetAllMembersAsync(string? searchTerm, int page = 1, int pageSize = 10,
                                                CancellationToken ct = default);

    Task<MemberDto[]> GetAllMemberDtosAsync(string? searchTerm, CancellationToken ct = default);

    Task<MemberDto?> GetMemberByIdAsync(Guid id, CancellationToken ct = default);
    Task<MemberDto> CreateMemberAsync(MemberCreateDto dto, CancellationToken ct = default);
    Task UpdateMemberAsync(Guid id, MemberUpdateDto dto, CancellationToken ct = default);
    Task DeleteMemberAsync(Guid id, CancellationToken ct = default);
}
