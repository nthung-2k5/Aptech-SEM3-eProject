using GiveAID.Models;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;

namespace GiveAID.Services;

public class MemberService : IMemberService
{
    public Task<MemberSummaryDto[]> GetAllMembersAsync(CancellationToken ct = default) =>
            Task.FromResult<MemberSummaryDto[]>(MockData.Members);

    public Task<MemberDto?> GetMemberByIdAsync(Guid id, CancellationToken ct = default) =>
            Task.FromResult(MockData.Members.FirstOrDefault(m => m.Id == id));

    public Task CreateMemberAsync(MemberSaveDto member, CancellationToken ct = default) =>
            throw new NotImplementedException();

    public Task UpdateMemberAsync(Guid id, MemberSaveDto member, CancellationToken ct = default) =>
            throw new NotImplementedException();

    public Task DeleteMemberAsync(Guid id, CancellationToken ct = default) => throw new NotImplementedException();
}
